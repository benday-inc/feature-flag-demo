using Benday.Presidents.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.Presidents.Api.DataAccess
{
    public class Person : Int32Identity
    {
        private static readonly DateTime DEFAULT_DATE_VALUE = DateTime.MinValue;

        public Person()
        {
            FirstName = String.Empty;
            LastName = String.Empty;
            Relationships = new List<Relationship>();
            Facts = new List<PersonFact>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual List<PersonFact> Facts
        {
            get;
            set;
        }

        public virtual List<Relationship> Relationships
        {
            get;
            set;
        }

        public void AddRelationship(string relationshipType, Person person)
        {
            if (string.IsNullOrEmpty(relationshipType))
                throw new ArgumentException("relationshipType is null or empty.", "relationshipType");
            if (person == null)
                throw new ArgumentNullException("person", "person is null.");

            var relationship = new Relationship();

            relationship.ToPerson = person;
            relationship.FromPerson = this;
            relationship.RelationshipType = relationshipType;

            Relationships.Add(relationship);
        }

        public void AddFact(string factType, string factValue)
        {
            AddFact(0, factType, factValue, DEFAULT_DATE_VALUE, DEFAULT_DATE_VALUE);
        }

        public void AddFact(string factType, DateTime factDate)
        {
            AddFact(factType, factDate, factDate);
        }

        public void AddFact(int id, string factType, DateTime factStartDate, DateTime factEndDate)
        {
            AddFact(id, factType, factType, factStartDate, factEndDate);
        }

        public void AddFact(string factType, DateTime factStartDate, DateTime factEndDate)
        {
            AddFact(0, factType, factType, factStartDate, factEndDate);
        }

        private bool AllowDuplicateFactType(string factType)
        {
            if (factType == PresidentsConstants.President)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveFact(int id)
        {
            if (id == 0)
            {
                return;
            }
            else
            {
                var match = Facts.Where(fact => fact.Id == id).FirstOrDefault();

                if (match != null)
                {
                    Facts.Remove(match);
                }
            }
        }

        private void UpdateExistingFactById(int id, string factType, string factValue, DateTime factStartDate, DateTime factEndDate)
        {
            PersonFact fact = null;

            bool foundIt = false;

            if (id != 0)
            {
                // locate existing fact 
                fact = (from temp in Facts
                        where temp.Id == id
                        select temp).FirstOrDefault();
            }

            if (fact == null)
            {
                fact = new PersonFact();

                fact.Person = this;
                fact.Id = id;
            }
            else
            {
                foundIt = true;
            }

            fact.FactType = factType;
            fact.FactValue = factValue;
            fact.StartDate = factStartDate;
            fact.EndDate = factEndDate;

            if (foundIt == false)
            {
                Facts.Add(fact);
            }
        }

        private void UpdateExistingFactByFactType(int id, string factType, string factValue, DateTime factStartDate, DateTime factEndDate)
        {
            bool foundIt = false;
            
            // locate existing fact 
            PersonFact fact = (from temp in Facts
                        where temp.FactType == factType
                        select temp).FirstOrDefault();
            
            if (fact == null)
            {
                fact = new PersonFact();

                fact.Person = this;
                fact.Id = id;
            }
            else
            {
                foundIt = true;
            }

            fact.FactType = factType;
            fact.FactValue = factValue;
            fact.StartDate = factStartDate;
            fact.EndDate = factEndDate;

            if (foundIt == false)
            {
                Facts.Add(fact);
            }
        }

        public void AddNewFact(int id, 
            string factType, 
            string factValue, 
            DateTime factStartDate, 
            DateTime factEndDate)
        {
            var fact = new PersonFact();

            fact.Person = this;
            fact.Id = id;

            fact.FactType = factType;
            fact.FactValue = factValue;
            fact.StartDate = factStartDate;
            fact.EndDate = factEndDate;

            Facts.Add(fact);
        }

        public void AddFact(string factType, string factValue, DateTime factStartDate, DateTime factEndDate)
        {
            AddFact(0, factType, factValue, factStartDate, factEndDate);
        }

        public void AddFact(int id, 
            string factType, 
            string factValue, 
            DateTime factStartDate, 
            DateTime factEndDate)
        {
            if (string.IsNullOrEmpty(factType))
                throw new ArgumentException("factType is null or empty.", "factType");
            
            if (factValue == null)
            {
                throw new ArgumentNullException("factValue", "Argument cannot be null.");
            }

            if (id != 0)
            {
                UpdateExistingFactById(id, factType, factValue, factStartDate, factEndDate);
            }
            else if (AllowDuplicateFactType(factType) == false)
            {
                UpdateExistingFactByFactType(id, factType, factValue, factStartDate, factEndDate);
            }
            else
            {
                AddNewFact(id, factType, factValue, factStartDate, factEndDate);
            }            
        }        
    }
}
