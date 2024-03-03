using Benday.Presidents.Api;
using Benday.Presidents.Api.DataAccess;
using Benday.Presidents.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.Presidents.UnitTests
{
    public static class UnitTestUtility
    {
        public static President GetThomasJeffersonAsPresident()
        {
            var info = new President();

            info.FirstName = "Thomas";
            info.LastName = "Jefferson";

            info.ImageFilename = "tommy-jeffs.jpg";

            info.AddTerm(
                "President",
                new DateTime(1801, 3, 4),
                new DateTime(1809, 3, 4),
                3);

            info.BirthDate = new DateTime(1743, 4, 13);
            info.DeathDate = new DateTime(1826, 7, 4);

            info.BirthCity = "Shadwell";
            info.BirthState = "Virginia";

            info.DeathCity = "Charlottesville";
            info.DeathState = "Virginia";

            return info;
        }

        public static President GetThomasJeffersonAsPresident(bool simulateAllValuesAreSaved)
        {
            var returnValue = GetThomasJeffersonAsPresident();

            if (simulateAllValuesAreSaved == true)
            {
                int simulatedIdentityValue = 2000;

                returnValue.Id = simulatedIdentityValue++;

                returnValue.Terms.ForEach(item => item.Id = simulatedIdentityValue++);
            }

            return returnValue;
        }

        public static Person GetThomasJeffersonAsPerson()
        {
            var info = new Person();

            info.FirstName = "Thomas";
            info.LastName = "Jefferson";

            info.AddFact(
                PresidentsConstants.President,
                "3",
                new DateTime(1801, 3, 4),
                new DateTime(1809, 3, 4));

            info.AddFact(PresidentsConstants.ImageFilename, "tommy-jeffs.jpg");

            info.AddFact(PresidentsConstants.BirthDate, new DateTime(1743, 4, 13));
            info.AddFact(PresidentsConstants.DeathDate, new DateTime(1826, 7, 4));

            info.AddFact(PresidentsConstants.BirthCity, "Shadwell");
            info.AddFact(PresidentsConstants.DeathCity, "Virginia");

            info.AddFact(PresidentsConstants.BirthState, "Charlottesville");
            info.AddFact(PresidentsConstants.DeathState, "Virginia");

            return info;
        }

        public static Person GetGroverClevelandAsPerson(bool simulateAllValuesAreSaved)
        {
            var returnValue = GetGroverClevelandAsPerson();

            if (simulateAllValuesAreSaved == true)
            {
                int simulatedIdentityValue = 1000;

                returnValue.Id = simulatedIdentityValue++;

                returnValue.Facts.ForEach(item => item.Id = simulatedIdentityValue++);
                returnValue.Relationships.ForEach(item => item.Id = simulatedIdentityValue++);
            }

            return returnValue;
        }

        public static Person GetGroverClevelandAsPerson()
        {
            var info = new Person();

            info.FirstName = "Grover";
            info.LastName = "Cleveland";

            info.AddFact(PresidentsConstants.ImageFilename, "grover-cleveland.jpg");

            info.AddFact(
                PresidentsConstants.President,
                "22",
                new DateTime(1885, 3, 4),
                new DateTime(1889, 3, 4));

            info.AddFact(
                PresidentsConstants.President,
                "24",
                new DateTime(1893, 3, 4),
                new DateTime(1897, 3, 4));

            info.AddFact(PresidentsConstants.BirthDate, new DateTime(1837, 3, 18));
            info.AddFact(PresidentsConstants.DeathDate, new DateTime(1908, 6, 24));

            info.AddFact(PresidentsConstants.BirthCity, "Caldwell");
            info.AddFact(PresidentsConstants.DeathCity, "New Jersey");

            info.AddFact(PresidentsConstants.BirthState, "Princeton");
            info.AddFact(PresidentsConstants.DeathState, "New Jersey");

            return info;
        }

        public static President GetGroverClevelandAsPresident(bool simulateAllValuesAreSaved)
        {
            var returnValue = GetGroverClevelandAsPresident();

            if (simulateAllValuesAreSaved == true)
            {
                int simulatedIdentityValue = 1000;

                returnValue.Id = simulatedIdentityValue++;

                returnValue.Terms.ForEach(item => item.Id = simulatedIdentityValue++);
            }

            return returnValue;
        }

        public static President GetGroverClevelandAsPresident()
        {
            var info = new President();

            info.FirstName = "Grover";
            info.LastName = "Cleveland";

            info.ImageFilename = "grover.jpg";

            info.AddTerm(
                "President",
                new DateTime(1885, 3, 4),
                new DateTime(1889, 3, 4), 
                22);

            info.AddTerm(
                "President",
                new DateTime(1893, 3, 4),
                new DateTime(1897, 3, 4),
                24);

            info.BirthDate = new DateTime(1837, 3, 18);
            info.DeathDate = new DateTime(1908, 6, 24);

            info.BirthCity = "Caldwell";
            info.BirthState = "New Jersey";

            info.DeathCity = "Princeton";
            info.DeathState = "New Jersey";

            return info;
        }


        public static void AssertAreEqual(Person expected, President actual)
        {
            Assert.AreEqual<int>(expected.Id, actual.Id, "Id");
            Assert.AreEqual<string>(expected.FirstName, actual.FirstName, "FirstName");
            Assert.AreEqual<string>(expected.LastName, actual.LastName, "LastName");

            AssertTerms(expected, actual);

            AssertValue(expected, PresidentsConstants.BirthCity, actual.BirthCity);
            AssertValue(expected, PresidentsConstants.BirthState, actual.BirthState);
            AssertValue(expected, PresidentsConstants.BirthDate, actual.BirthDate);

            AssertValue(expected, PresidentsConstants.DeathCity, actual.DeathCity);
            AssertValue(expected, PresidentsConstants.DeathState, actual.DeathState);
            AssertValue(expected, PresidentsConstants.DeathDate, actual.DeathDate);

            AssertValue(expected, PresidentsConstants.ImageFilename, actual.ImageFilename);
        }

        public static void AssertAreEqual(President expected, Person actual)
        {
            Assert.AreEqual<int>(expected.Id, actual.Id, "Id");
            Assert.AreEqual<string>(expected.FirstName, actual.FirstName, "FirstName");
            Assert.AreEqual<string>(expected.LastName, actual.LastName, "LastName");

            AssertTerms(expected, actual);

            AssertValue(expected.BirthCity, PresidentsConstants.BirthCity, actual);
            AssertValue(expected.BirthState, PresidentsConstants.BirthState, actual);
            AssertValue(expected.BirthDate, PresidentsConstants.BirthDate, actual);

            AssertValue(expected.DeathCity, PresidentsConstants.DeathCity, actual);
            AssertValue(expected.DeathState, PresidentsConstants.DeathState, actual);
            AssertValue(expected.DeathDate, PresidentsConstants.DeathDate, actual);

            AssertValue(expected.ImageFilename, PresidentsConstants.ImageFilename, actual);
        }

        public static void AssertTerms(Person expected, President actual)
        {
            var expectedFacts = expected.Facts.GetFacts(PresidentsConstants.President);

            Assert.AreEqual<int>(actual.Terms.Count, expectedFacts.Count, "Term count and fact count didn't match.");

            if (expectedFacts.Count > 0)
            {
                for (int i = 0; i < expectedFacts.Count; i++)
                {
                    AssertAreEqual(expectedFacts[i], actual.Terms[i]);
                }
            }
        }

        public static void AssertTerms(President expected, Person actual)
        {
            var actualFactsPresidentRole = actual.Facts.GetFacts(PresidentsConstants.President);

            Assert.AreEqual<int>(expected.Terms.Count, 
                actualFactsPresidentRole.Count, 
                "Term count and fact count didn't match.");

            if (expected.Terms.Count > 0)
            {
                for (int i = 0; i < expected.Terms.Count; i++)
                {
                    AssertAreEqual(expected.Terms[i], actualFactsPresidentRole[i]);                    
                }
            }
        }

        public static void AssertAreEqual(PersonFact expected, Term actual)
        {
            if (expected == null)
                throw new ArgumentNullException("expected", "expected is null.");
            if (actual == null)
                throw new ArgumentNullException("actual", "actual is null.");

            Assert.AreEqual<int>(expected.Id, actual.Id, "Id");
            Assert.AreEqual<string>(expected.FactType, actual.Role, "FactType");
            Assert.AreEqual<DateTime>(expected.StartDate, actual.Start, "Start");
            Assert.AreEqual<DateTime>(expected.EndDate, actual.End, "End");
            Assert.AreEqual<string>(
                expected.FactValue, 
                actual.Number.ToString(),
                "President number did not match fact value.");
        }

        public static void AssertAreEqual(Term expected, PersonFact actual)
        {
            if (expected == null)
                throw new ArgumentNullException("expected", "expected is null.");
            if (actual == null)
                throw new ArgumentNullException("actual", "actual is null.");

            Assert.AreEqual<int>(expected.Id, actual.Id, "Id");
            Assert.AreEqual<string>(expected.Role, actual.FactType, "Role");
            Assert.AreEqual<DateTime>(expected.Start, actual.StartDate, "Start");
            Assert.AreEqual<DateTime>(expected.End, actual.EndDate, "End");
            Assert.AreEqual<string>(expected.Number.ToString(), 
                actual.FactValue, 
                "President number did not match fact value.");
        }

        public static void AssertAreEqual(DateTime expected, PersonFact actual)
        {
            Assert.AreEqual<DateTime>(expected, actual.StartDate, "StartDate was wrong.");
            Assert.AreEqual<DateTime>(expected, actual.EndDate, "EndDate was wrong.");
        }

        public static void AssertAreEqual(PersonFact expectedFact, DateTime actualValue)
        {
            Assert.AreEqual<DateTime>(expectedFact.StartDate, actualValue, "StartDate was wrong.");
        }

        public static void AssertAreEqual(string expected, PersonFact actual)
        {
            Assert.AreEqual<string>(expected, actual.FactValue, "FactValue was wrong.");
        }

        public static void AssertAreEqual(PersonFact expectedFact, string actualValue)
        {
            Assert.AreEqual<string>(expectedFact.FactValue, actualValue, "FactValue didn't match.");
        }

        public static void AssertValue(DateTime expectedValue, string expectedKey, Person actual)
        {
            var actualFact = actual.Facts.GetFact(expectedKey);

            Assert.IsNotNull(actualFact, "Could not find fact named '{0}'.", expectedKey);

            AssertAreEqual(expectedValue, actualFact);
        }

        public static void AssertValue(string expectedValue, string expectedKey, Person actual)
        {
            var actualFact = actual.Facts.GetFact(expectedKey);

            Assert.IsNotNull(actualFact, "Could not find fact named '{0}'.", expectedKey);

            AssertAreEqual(expectedValue, actualFact);
        }

        public static void AssertValue(Person expected, string key, string actualValue)
        {
            var expectedFact = expected.Facts.GetFact(key);

            Assert.IsNotNull(expectedFact, "Could not find fact named '{0}'.", key);

            AssertAreEqual(expectedFact, actualValue);
        }

        public static void AssertValue(Person expected, string key, DateTime actualValue)
        {
            var expectedFact = expected.Facts.GetFact(key);

            Assert.IsNotNull(expectedFact, "Could not find fact named '{0}'.", key);

            AssertAreEqual(expectedFact, actualValue);
        }

        public static T GetModel<T>(ActionResult actionResult) where T : class
        {
            var asViewResult = actionResult as ViewResult;

            return asViewResult.Model as T;
        }

        public static void AssertIsHttpNotFound(ActionResult actionResult)
        {            
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }
    }
}
