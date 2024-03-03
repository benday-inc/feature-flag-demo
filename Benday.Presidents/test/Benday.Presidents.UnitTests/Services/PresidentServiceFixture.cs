using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benday.Presidents.Api.Services;
using Benday.Presidents.Api.Models;
using Benday.Presidents.Api;
using System.Collections.Generic;
using Benday.Presidents.Api.DataAccess;

namespace Benday.Presidents.UnitTests.Services
{
    [TestClass]
    public class PresidentServiceFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
            _PersonRepositoryInstance = null;
            _ValidatorStrategyInstance = null;
        }

        private PresidentService _SystemUnderTest;

        private PresidentService SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = 
                        new PresidentService(
                            PersonRepositoryInstance,
                            ValidatorStrategyInstance);
                }

                return _SystemUnderTest;
            }
        }

        private MockPresidentValidatorStrategy _ValidatorStrategyInstance;
        public MockPresidentValidatorStrategy ValidatorStrategyInstance
        {
            get
            {
                if (_ValidatorStrategyInstance == null)
                {
                    _ValidatorStrategyInstance = new MockPresidentValidatorStrategy();
                }

                return _ValidatorStrategyInstance;
            }
        }
        

        private InMemoryRepository<Person> _PersonRepositoryInstance;
        public InMemoryRepository<Person> PersonRepositoryInstance
        {
            get
            {
                if (_PersonRepositoryInstance == null)
                {
                    _PersonRepositoryInstance = new InMemoryRepository<Person>();
                }

                return _PersonRepositoryInstance;
            }
        }


        private void PopulateRepositoryWithTestData()
        {
            PersonRepositoryInstance.Save(UnitTestUtility.GetGroverClevelandAsPerson(true));
            PersonRepositoryInstance.Save(UnitTestUtility.GetThomasJeffersonAsPerson());

            var personWithNoFacts = new Person()
            {
                FirstName = "Skippy",
                LastName = "DefinitelyNotPresident"
            };
            PersonRepositoryInstance.Save(personWithNoFacts);

            var personWhoWasVP = new Person()
            {
                FirstName = "Al",
                LastName = "Gore"
            };

            personWhoWasVP.AddFact(PresidentsConstants.VicePresident, 
                new DateTime(1992, 1, 1), 
                new DateTime(2000, 1, 1));

            PersonRepositoryInstance.Save(personWhoWasVP);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GivenAnInvalidPresidentWhenSaveIsCalledThenThrowAnException()
        {
            ValidatorStrategyInstance.IsValidReturnValue = false;

            SystemUnderTest.Save(new President());
        }

        [TestMethod]
        public void GetAllPresidentsOnlyReturnsPresidents()
        {
            PopulateRepositoryWithTestData();

            IList<President> actual = SystemUnderTest.GetPresidents();

            Assert.AreEqual<int>(2, actual.Count, "Wrong number of presidents.");

            var lastNames =
                (from temp in actual
                 select temp.LastName).ToList();

            Assert.IsTrue(lastNames.Contains("Cleveland"));
            Assert.IsTrue(lastNames.Contains("Jefferson"));
        }

        [TestMethod]
        public void SearchByFirstName()
        {
            PopulateRepositoryWithTestData();

            IList<President> actual = SystemUnderTest.Search("Grover", String.Empty);

            Assert.AreEqual<int>(1, actual.Count, "Wrong number of presidents.");

            var lastNames =
                (from temp in actual
                 select temp.LastName).ToList();

            Assert.IsTrue(lastNames.Contains("Cleveland"));
        }

        [TestMethod]
        public void SearchByFirstNamePartial()
        {
            PopulateRepositoryWithTestData();

            IList<President> actual = SystemUnderTest.Search("ove", String.Empty);

            Assert.AreEqual<int>(1, actual.Count, "Wrong number of presidents.");

            var lastNames =
                (from temp in actual
                 select temp.LastName).ToList();

            Assert.IsTrue(lastNames.Contains("Cleveland"));
        }

        [TestMethod]
        public void SearchByLastName()
        {
            PopulateRepositoryWithTestData();

            IList<President> actual = SystemUnderTest.Search(String.Empty, "Cleveland");

            Assert.AreEqual<int>(1, actual.Count, "Wrong number of presidents.");

            var lastNames =
                (from temp in actual
                 select temp.LastName).ToList();

            Assert.IsTrue(lastNames.Contains("Cleveland"));
        }

        [TestMethod]
        public void SearchByLastNamePartial()
        {
            PopulateRepositoryWithTestData();

            IList<President> actual = SystemUnderTest.Search(String.Empty, "eve");

            Assert.AreEqual<int>(1, actual.Count, "Wrong number of presidents.");

            var lastNames =
                (from temp in actual
                 select temp.LastName).ToList();

            Assert.IsTrue(lastNames.Contains("Cleveland"));
        }

        [TestMethod]
        public void SearchByFirstNameLastName()
        {
            PopulateRepositoryWithTestData();

            IList<President> actual = SystemUnderTest.Search("Grover", "Cleveland");

            Assert.AreEqual<int>(1, actual.Count, "Wrong number of presidents.");

            var lastNames =
                (from temp in actual
                 select temp.LastName).ToList();

            Assert.IsTrue(lastNames.Contains("Cleveland"));
        }

        [TestMethod]
        public void SearchByFirstNameLastNamePartial()
        {
            PopulateRepositoryWithTestData();

            IList<President> actual = SystemUnderTest.Search("ove", "eve");

            Assert.AreEqual<int>(1, actual.Count, "Wrong number of presidents.");

            var lastNames =
                (from temp in actual
                 select temp.LastName).ToList();

            Assert.IsTrue(lastNames.Contains("Cleveland"));
        }

        [TestMethod]
        public void SearchByFirstNameLastNameCaseInsensitive()
        {
            PopulateRepositoryWithTestData();

            IList<President> actual = SystemUnderTest.Search("grover", "cleveland");

            Assert.AreEqual<int>(1, actual.Count, "Wrong number of presidents.");

            var lastNames =
                (from temp in actual
                 select temp.LastName).ToList();

            Assert.IsTrue(lastNames.Contains("Cleveland"));
        }

        [TestMethod]
        public void SearchByPartialFirstNameLastName()
        {
            PopulateRepositoryWithTestData();

            IList<President> actual = SystemUnderTest.Search("Gro", "Cle");

            Assert.AreEqual<int>(1, actual.Count, "Wrong number of presidents.");

            var lastNames =
                (from temp in actual
                 select temp.LastName).ToList();

            Assert.IsTrue(lastNames.Contains("Cleveland"));
        }

        [TestMethod]
        public void SearchByEmptyFirstNameLastNameReturnsAll()
        {
            PopulateRepositoryWithTestData();

            IList<President> actual = SystemUnderTest.Search(
                String.Empty, String.Empty);

            Assert.AreEqual<int>(2, actual.Count, "Wrong number of presidents.");            
        }

        [TestMethod]
        public void SearchByNullFirstNameLastNameReturnsAll()
        {
            PopulateRepositoryWithTestData();

            IList<President> actual = SystemUnderTest.Search(
                null, null);

            Assert.AreEqual<int>(2, actual.Count, "Wrong number of presidents.");
        }

        [TestMethod]
        public void SearchByBogusFirstNameLastNameReturnsNoResults()
        {
            PopulateRepositoryWithTestData();

            IList<President> actual = SystemUnderTest.Search("Thomas", "Cleveland");

            Assert.AreEqual<int>(0, actual.Count, "Wrong number of presidents.");
        }

        [TestMethod]
        public void WhenSaveIsCalledThenIdIsNotZeroAndIsInRepository()
        {
            President saveThis = UnitTestUtility.GetGroverClevelandAsPresident();

            SystemUnderTest.Save(saveThis);

            Assert.AreNotEqual<int>(0, saveThis.Id, "Id should not be zero after save.");

            var actual = PersonRepositoryInstance.GetById(saveThis.Id);

            Assert.IsNotNull(actual, "Person wasn't saved to repository.");
        }

        [TestMethod]
        public void WhenSaveIsCalledUsingAnExistingPresidentModificationsAreSavedInRepository()
        {
            PopulateRepositoryWithTestData();

            Person existingPerson = GetPresidentByNameFromTestRepository("Cleveland");

            President existingPresident = SystemUnderTest.GetPresidentById(existingPerson.Id);

            ModifyValues(existingPresident);

            SystemUnderTest.Save(existingPresident);

            UnitTestUtility.AssertAreEqual(existingPresident, existingPerson);
        }

        [TestMethod]
        public void WhenSaveIsCalledUsingAnExistingPresidentThenDeletedTermsAreRemovedFromCollection()
        {
            PopulateRepositoryWithTestData();

            Person existingPerson = GetPresidentByNameFromTestRepository("Cleveland");

            President existingPresident = SystemUnderTest.GetPresidentById(existingPerson.Id);

            var existingTerm0 = existingPresident.Terms[0];

            existingTerm0.IsDeleted = true;

            SystemUnderTest.Save(existingPresident);

            Assert.AreEqual<int>(1, existingPresident.Terms.Count, "Wrong number of terms.");
            Assert.IsFalse(existingPresident.Terms.Contains(existingTerm0), "Should not contain deleted term.");
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenSaveIsCalledUsingAPresidentWithAnInvalidIdThenAnExceptionIsThrown()
        {
            President presidentWithFakeId = UnitTestUtility.GetGroverClevelandAsPresident();

            presidentWithFakeId.Id = 12341234;

            SystemUnderTest.Save(presidentWithFakeId);
        }

        private void ModifyValues(President existingPresident)
        {
            existingPresident.BirthCity = "Lollipop";
            existingPresident.BirthState = "Missouri";

            existingPresident.BirthDate = new DateTime(1954, 6, 22);
            existingPresident.DeathDate = new DateTime(1982, 11, 14);

            existingPresident.DeathCity = "Gurgle";
            existingPresident.DeathState = "Montana";

            existingPresident.FirstName = "Grovegrove";
            existingPresident.LastName = "Washington, Jr.";

            existingPresident.Terms[0].Start = new DateTime(1977, 8, 29);
            existingPresident.Terms[0].End = new DateTime(1981, 5, 3);
        }

        [TestMethod]
        public void GetPresidentById()
        {
            PopulateRepositoryWithTestData();

            Person expected = GetPresidentByNameFromTestRepository("Jefferson");

            President actual = SystemUnderTest.GetPresidentById(expected.Id);

            Assert.IsNotNull(actual, "Null president was returned.");

            Assert.AreEqual<int>(expected.Id, actual.Id, "Id");
            Assert.AreEqual<string>(expected.LastName, actual.LastName, "LastName");
        }

        [TestMethod]
        public void DeletePresidentByIdRemovesPresidentFromRepository()
        {
            PopulateRepositoryWithTestData();

            Person expected = GetPresidentByNameFromTestRepository("Jefferson");

            SystemUnderTest.DeletePresidentById(expected.Id);

            Assert.IsNull(PersonRepositoryInstance.GetById(expected.Id),
                "Value should have been deleted.");
        }

        private Person GetPresidentByNameFromTestRepository(string lastName)
        {
            var match = (from temp in PersonRepositoryInstance.Items
                         where temp.LastName == lastName
                         select temp
                         ).FirstOrDefault();

            return match;
        }
    }
}
