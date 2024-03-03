using System;
using System.Collections.Generic;
using System.Linq;
using Benday.Presidents.Api.Models;
using Benday.Presidents.Api.DataAccess;

namespace Benday.Presidents.Api.Services
{
    public class PersonToPresidentAdapter
    {

        public PersonToPresidentAdapter()
        {

        }

        public void Adapt(IEnumerable<Person> fromValues, IList<President> toValues)
        {
            if (fromValues == null)
                throw new ArgumentNullException("fromValues", "fromValues is null.");

            President toValue;

            foreach (var fromValue in fromValues)
            {
                toValue = new President();

                Adapt(fromValue, toValue);

                toValues.Add(toValue);
            }
        }

        public void Adapt(Person fromValue, President toValue)
        {
            if (fromValue == null)
                throw new ArgumentNullException("fromValue", "fromValue is null.");
            if (toValue == null)
                throw new ArgumentNullException("toValue", "toValue is null.");

            toValue.Id = fromValue.Id;
            toValue.FirstName = fromValue.FirstName;
            toValue.LastName = fromValue.LastName;

            toValue.ImageFilename = fromValue.Facts.GetFactValueAsString(
                PresidentsConstants.ImageFilename);

            toValue.BirthCity = fromValue.Facts.GetFactValueAsString(
                PresidentsConstants.BirthCity);

            toValue.BirthDate = fromValue.Facts.GetFactValueAsDateTime(
                PresidentsConstants.BirthDate);

            toValue.BirthState = fromValue.Facts.GetFactValueAsString(
                PresidentsConstants.BirthState);

            toValue.DeathCity = fromValue.Facts.GetFactValueAsString(
                PresidentsConstants.DeathCity);

            toValue.DeathDate = fromValue.Facts.GetFactValueAsDateTime(
                PresidentsConstants.DeathDate);

            toValue.DeathState = fromValue.Facts.GetFactValueAsString(
                PresidentsConstants.DeathState);

            foreach (var fromFact in
                fromValue.Facts.GetFacts(PresidentsConstants.President))
            {
                var temp = new Term();

                temp.Id = fromFact.Id;
                temp.Role = fromFact.FactType;
                temp.Start = fromFact.StartDate;
                temp.End = fromFact.EndDate;
                temp.Number = SafeToInt32(fromFact.FactValue, -1);

                toValue.Terms.Add(temp);
            }
        }

        private int SafeToInt32(string valueAsString, int defaultInt32Value)
        {
            int temp;

            if (Int32.TryParse(valueAsString, out temp) == false)
            {
                return defaultInt32Value;
            }
            else
            {
                return temp;
            }
        }

        private void AdaptFacts(President fromValue, Person toValue)
        {
            AdaptValueToPersonFact(fromValue.ImageFilename,
                            toValue,
                            PresidentsConstants.ImageFilename);

            AdaptValueToPersonFact(fromValue.BirthCity,
                toValue,
                PresidentsConstants.BirthCity);

            AdaptValueToPersonFact(fromValue.BirthDate,
                toValue,
                PresidentsConstants.BirthDate);

            AdaptValueToPersonFact(fromValue.BirthState,
                toValue,
                PresidentsConstants.BirthState);

            AdaptValueToPersonFact(fromValue.DeathCity,
                toValue,
                PresidentsConstants.DeathCity);

            AdaptValueToPersonFact(fromValue.DeathDate,
                toValue,
                PresidentsConstants.DeathDate);

            AdaptValueToPersonFact(fromValue.DeathState,
                toValue,
                PresidentsConstants.DeathState);
        }

        public void Adapt(President fromValue, Person toValue)
        {
            if (fromValue == null)
                throw new ArgumentNullException("fromValue", "fromValue is null.");
            if (toValue == null)
                throw new ArgumentNullException("toValue", "toValue is null.");

            toValue.Id = fromValue.Id;
            toValue.FirstName = fromValue.FirstName;
            toValue.LastName = fromValue.LastName;

            if (fromValue.Id == 0)
            {
                toValue.Facts.Clear();
            }

            AdaptFacts(fromValue, toValue);

            AdaptTerms(fromValue, toValue);
        }

        private static void AdaptTerms(President fromValue, Person toValue)
        {
            foreach (var fromTerm in fromValue.Terms)
            {
                if (fromTerm.IsDeleted == false)
                {
                    toValue.AddFact(fromTerm.Id,
                        fromTerm.Role,
                        fromTerm.Number.ToString(),
                        fromTerm.Start,
                        fromTerm.End
                        );
                }
                else if (fromTerm.IsDeleted == true && fromTerm.Id > 0)
                {
                    toValue.RemoveFact(fromTerm.Id);
                }
            }
        }
        public void AdaptValueToPersonFact(string fromValue,
        Person toPerson, 
            string toPersonFactType)
        {
            toPerson.AddFact(toPersonFactType, fromValue);
        }

        public void AdaptValueToPersonFact(DateTime fromValue,
            Person toPerson,
            string toPersonFactType)
        {
            toPerson.AddFact(toPersonFactType, fromValue);
        }
    }
}
