using System;
using System.Collections.Generic;
using System.Linq;
using Benday.Presidents.Api.Models;
using Benday.Presidents.Common;
using Benday.Presidents.Api.DataAccess;

namespace Benday.Presidents.Api.Services
{
    public class PresidentServiceWithLogging : IPresidentService
    {
        private IRepository<Person> _RepositoryInstance;
        private IPresidentValidatorStrategy _ValidatorInstance;

        private PersonToPresidentAdapter _Adapter;

        private ILogger _Logger;

        public PresidentServiceWithLogging(
            IRepository<Person> personRepositoryInstance,
            IPresidentValidatorStrategy validator,
            ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException("logger", "logger is null.");
            if (personRepositoryInstance == null)
                throw new ArgumentNullException("personRepositoryInstance", "personRepositoryInstance is null.");
            if (validator == null)
                throw new ArgumentNullException("validator", "Argument cannot be null.");
            
            _RepositoryInstance = personRepositoryInstance;
            _ValidatorInstance = validator;
            _Adapter = new PersonToPresidentAdapter();
            _Logger = logger;
        }

        public void Save(President saveThis)
        {
            if (saveThis == null)
                throw new ArgumentNullException("saveThis", "saveThis is null.");

            if (_ValidatorInstance.IsValid(saveThis) == false)
            {
                throw new InvalidOperationException("President is invalid.");
            }

            Person toValue;

            if (saveThis.Id == 0)
            {
                toValue = new Person();
            }
            else
            {
                toValue = _RepositoryInstance.GetById(saveThis.Id);

                if (toValue == null)
                {
                    throw new InvalidOperationException(
                        $"Could not locate a person with an id of '{saveThis.Id}'."
                        );
                }
            }

            _Adapter.Adapt(saveThis, toValue);

            _Logger.LogFeatureUsage("President.Save");
            _RepositoryInstance.Save(toValue);

            // remove terms that are marked for delete
            saveThis.Terms
                .Where(term => term.IsDeleted == true)
                .ToList()
                .ForEach(term => saveThis.Terms.Remove(term));

            saveThis.Id = toValue.Id;
        }

        public void DeletePresidentById(int id)
        {
            var match = _RepositoryInstance.GetById(id);

            if (match == null)
            {
                throw new InvalidOperationException(
                        $"Could not locate a person with an id of '{id}'."
                        );
            }
            else
            {
                _Logger.LogFeatureUsage("President.Delete");
                _RepositoryInstance.Delete(match);
            }
        }

        public President GetPresidentById(int id)
        {
            _Logger.LogFeatureUsage("President.GetById");
            var match = _RepositoryInstance.GetById(id);

            if (match == null)
            {
                return null;
            }
            else
            {
                return ToPresident(match);
            }
        }

        public IList<President> GetPresidents()
        {
            return GetPresidents(true);
        }

        private IList<President> GetPresidents(bool withLogFeatureUsage)
        {
            if (withLogFeatureUsage == true)
            {
                _Logger.LogFeatureUsage("President.GetAll");
            }

            var allPeople = _RepositoryInstance.GetAll();

            var peopleWhoWerePresident =
                (
                from temp in allPeople
                where temp.Facts.GetFact(PresidentsConstants.President) != null
                select temp
                );

            return ToPresidents(peopleWhoWerePresident);
        }

        private President ToPresident(Person fromValue)
        {
            var toValue = new President();

            _Adapter.Adapt(fromValue, toValue);

            return toValue;
        }

        private IList<President> ToPresidents(IEnumerable<Person> peopleWhoWerePresident)
        {
            var returnValues = new List<President>();

            _Adapter.Adapt(peopleWhoWerePresident, returnValues);

            return returnValues;
        }

        public IList<President> Search(
            string firstName, string lastName)
        {
            var allPresidents = GetPresidents(false);

            _Logger.LogFeatureUsage("President.SearchByFirstNameLastName");

            IEnumerable<President> returnValues =
                allPresidents;

            if (String.IsNullOrWhiteSpace(firstName) == false)
            {
                returnValues =
                    returnValues.Where(p => p.FirstName.ToLower().Contains(firstName.ToLower()));
            }

            if (String.IsNullOrWhiteSpace(lastName) == false)
            {
                returnValues =
                    returnValues.Where(p => p.LastName.ToLower().Contains(lastName.ToLower()));
            }

            return returnValues.ToList();
        }

        public IList<President> Search(string firstName, string lastName, string birthState, string deathState)
        {
            var allPresidents = GetPresidents();

            _Logger.LogFeatureUsage("President.SearchByFirstNameLastNameBirthStateDeathState");

            IEnumerable<President> returnValues =
                allPresidents;

            if (String.IsNullOrWhiteSpace(firstName) == false)
            {
                returnValues =
                    returnValues.Where(p => p.FirstName.ToLower().Contains(firstName.ToLower()));
            }

            if (String.IsNullOrWhiteSpace(lastName) == false)
            {
                returnValues =
                    returnValues.Where(p => p.LastName.ToLower().Contains(lastName.ToLower()));
            }

            if (String.IsNullOrWhiteSpace(birthState) == false)
            {
                returnValues =
                    returnValues.Where(p => p.BirthState != null && p.BirthState.ToLower().Contains(birthState.ToLower()));
            }

            if (String.IsNullOrWhiteSpace(deathState) == false)
            {
                returnValues =
                    returnValues.Where(p => p.DeathState != null && p.DeathState.ToLower().Contains(deathState.ToLower()));
            }

            return returnValues.ToList();
        }
    }
}
