using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benday.Presidents.Api.Models;
using Benday.Presidents.Api;
using Benday.Presidents.Api.DataAccess;

namespace Benday.Presidents.UnitTests.Models
{
    [TestClass]
    public class PersonFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private Person _SystemUnderTest;
        public Person SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new Person();
                }

                return _SystemUnderTest;
            }
        }

        [TestMethod]
        public void WhenCreatedThenFirstNameIsEmptyString()
        {
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.FirstName, "FirstName should be empty.");
        }

        [TestMethod]
        public void WhenCreatedThenLastNameIsEmptyString()
        {
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.LastName, "LastName should be empty.");
        }

        [TestMethod]
        public void WhenCreatedThenFactsCollectionIsNotNull()
        {
            Assert.IsNotNull(SystemUnderTest.Facts);
        }

        [TestMethod]
        public void WhenCreatedThenRelationshipsCollectionIsNotNull()
        {
            Assert.IsNotNull(SystemUnderTest.Relationships);
        }

        [TestMethod]
        public void WhenAddRelationshipIsCalledThenRelationshipIsAddedToCollection()
        {
            var toPerson = new Person();

            var expectedRelationshipType = "Vice President";

            SystemUnderTest.AddRelationship(expectedRelationshipType, toPerson);

            Assert.AreEqual<int>(1, SystemUnderTest.Relationships.Count, "Should be a relationship.");

            var actual = SystemUnderTest.Relationships[0];

            Assert.AreEqual<string>(expectedRelationshipType, actual.RelationshipType, "Relationship type was wrong.");
            Assert.AreSame(toPerson, actual.ToPerson, "ToPerson was wrong.");
            Assert.AreSame(SystemUnderTest, actual.FromPerson, "FromPerson was wrong.");
        }

        [TestMethod]
        public void WhenAddFactIsCalledThenFactIsAddedToCollection_StringBasedFact()
        {
            var expectedFactType = "Role";
            var expectedFactValue = "US Senator";

            SystemUnderTest.AddFact(expectedFactType, expectedFactValue);

            Assert.AreEqual<int>(1, SystemUnderTest.Facts.Count, "Should be a fact.");

            var actual = SystemUnderTest.Facts[0];

            Assert.AreEqual<string>(expectedFactType, actual.FactType, "Fact type was wrong.");
            Assert.AreEqual<string>(expectedFactValue, actual.FactValue, "Fact value was wrong.");
            Assert.AreSame(SystemUnderTest, actual.Person, "Person was wrong.");
            Assert.AreSame(SystemUnderTest, actual.Person, "Person was wrong.");
        }

        [TestMethod]
        public void WhenAddFactIsCalledWithNewValueThenFactIsUpdated_NonDuplicatingStringBasedFact()
        {
            var expectedFactType = "Role";
            var originalFactValue = "Janitor";
            var expectedFactValue = "US Senator";

            SystemUnderTest.AddFact(expectedFactType, originalFactValue);
            SystemUnderTest.AddFact(expectedFactType, expectedFactValue);

            Assert.AreEqual<int>(1, SystemUnderTest.Facts.Count, "Should only be one fact.");

            var actual = SystemUnderTest.Facts[0];

            Assert.AreEqual<string>(expectedFactType, actual.FactType, "Fact type was wrong.");
            Assert.AreEqual<string>(expectedFactValue, actual.FactValue, "Fact value was wrong.");
            Assert.AreSame(SystemUnderTest, actual.Person, "Person was wrong.");
            Assert.AreSame(SystemUnderTest, actual.Person, "Person was wrong.");
        }

        [TestMethod]
        public void WhenAddFactIsCalledThenFactIsAddedToCollection_DateBasedFact()
        {
            var expectedFactType = PresidentsConstants.BirthDate;
            var expectedFactValue = expectedFactType;
            var expectedFactDate = new DateTime(1928, 4, 3);

            SystemUnderTest.AddFact(expectedFactType, expectedFactDate);

            Assert.AreEqual<int>(1, SystemUnderTest.Facts.Count, "Should be a fact.");

            var actual = SystemUnderTest.Facts[0];

            Assert.AreEqual<string>(expectedFactType, actual.FactType, "Fact type was wrong.");
            Assert.AreEqual<string>(expectedFactValue, actual.FactValue, "Fact value was wrong.");
            Assert.AreEqual<DateTime>(expectedFactDate, actual.StartDate, "Fact start date was wrong.");
            Assert.AreEqual<DateTime>(expectedFactDate, actual.EndDate, "Fact end date was wrong.");
            Assert.AreSame(SystemUnderTest, actual.Person, "Person was wrong.");
        }

        [TestMethod]
        public void WhenAddFactIsCalledTwiceThenFactIsUpdated_NonDuplicatingDateBasedFact()
        {
            var expectedFactType = PresidentsConstants.BirthDate;
            var expectedFactValue = expectedFactType;
            var expectedFactDate = new DateTime(1928, 4, 3);

            SystemUnderTest.AddFact(expectedFactType, expectedFactDate.AddYears(-5));
            SystemUnderTest.AddFact(expectedFactType, expectedFactDate);

            Assert.AreEqual<int>(1, SystemUnderTest.Facts.Count, "Should be a fact.");

            var actual = SystemUnderTest.Facts[0];

            Assert.AreEqual<string>(expectedFactType, actual.FactType, "Fact type was wrong.");
            Assert.AreEqual<string>(expectedFactValue, actual.FactValue, "Fact value was wrong.");
            Assert.AreEqual<DateTime>(expectedFactDate, actual.StartDate, "Fact start date was wrong.");
            Assert.AreEqual<DateTime>(expectedFactDate, actual.EndDate, "Fact end date was wrong.");
            Assert.AreSame(SystemUnderTest, actual.Person, "Person was wrong.");
        }

        [TestMethod]
        public void WhenAddFactIsCalledThenFactIsAddedToCollection_DateRangeFact()
        {
            var expectedFactType = PresidentsConstants.President;
            var expectedFactValue = expectedFactType;
            var expectedFactStartDate = new DateTime(1928, 4, 3);
            var expectedFactEndDate = new DateTime(1929, 4, 3);

            SystemUnderTest.AddFact(expectedFactType, expectedFactStartDate, expectedFactEndDate);

            Assert.AreEqual<int>(1, SystemUnderTest.Facts.Count, "Should be a fact.");

            var actual = SystemUnderTest.Facts[0];

            Assert.AreEqual<string>(expectedFactType, actual.FactType, "Fact type was wrong.");
            Assert.AreEqual<string>(expectedFactValue, actual.FactValue, "Fact value was wrong.");
            Assert.AreEqual<DateTime>(expectedFactStartDate, actual.StartDate, "Fact start date was wrong.");
            Assert.AreEqual<DateTime>(expectedFactEndDate, actual.EndDate, "Fact end date was wrong.");
            Assert.AreSame(SystemUnderTest, actual.Person, "Person was wrong.");
        }

        [TestMethod]
        public void WhenAddFactIsCalledThenFactIsAddedToCollection_DateRangeFactThatAllowsDuplicates()
        {
            var expectedFactType = PresidentsConstants.President;
            var expectedFactValue = expectedFactType;
            var expectedFactStartDate = new DateTime(1928, 4, 3);
            var expectedFactEndDate = new DateTime(1929, 4, 3);

            SystemUnderTest.AddFact(expectedFactType, expectedFactStartDate, expectedFactEndDate);
            SystemUnderTest.AddFact(expectedFactType,
                expectedFactStartDate.AddYears(10),
                expectedFactEndDate.AddYears(10));

            Assert.AreEqual<int>(2, SystemUnderTest.Facts.Count, "Should be two facts.");
        }

        [TestMethod]
        public void WhenAddFactIsCalledWithNonZeroIdThenFactIsModified_DateRangeFactThatAllowsDuplicates()
        {
            var expectedFactType = PresidentsConstants.President;
            var expectedFactValue = expectedFactType;
            var expectedFactStartDate = new DateTime(1928, 4, 3);
            var expectedFactEndDate = new DateTime(1929, 4, 3);

            SystemUnderTest.AddFact(21,
                expectedFactType,
                expectedFactStartDate.AddDays(-10),
                expectedFactEndDate.AddDays(-10));

            SystemUnderTest.AddFact(27, expectedFactType,
                expectedFactStartDate.AddYears(10),
                expectedFactEndDate.AddYears(10));

            // modify an existing saved fact
            SystemUnderTest.AddFact(21,
                expectedFactType,
                expectedFactStartDate,
                expectedFactEndDate);

            Assert.AreEqual<int>(2, SystemUnderTest.Facts.Count, "Should be two facts.");

            var actual = SystemUnderTest.Facts[0];

            Assert.AreEqual<int>(21, actual.Id, "Wrong fact id.");
            Assert.AreEqual<string>(expectedFactType, actual.FactType, "Fact type was wrong.");
            Assert.AreEqual<string>(expectedFactValue, actual.FactValue, "Fact value was wrong.");
            Assert.AreEqual<DateTime>(expectedFactStartDate, actual.StartDate, "Fact start date was wrong.");
            Assert.AreEqual<DateTime>(expectedFactEndDate, actual.EndDate, "Fact end date was wrong.");
            Assert.AreSame(SystemUnderTest, actual.Person, "Person was wrong.");
        }

        [TestMethod]
        public void RemoveFactById()
        {
            var removeThisId = 123;
            var fact0 = new PersonFact() { Id = removeThisId };
            var fact1 = new PersonFact() { Id = 456 };

            SystemUnderTest.Facts.Add(fact0);
            SystemUnderTest.Facts.Add(fact1);

            SystemUnderTest.RemoveFact(removeThisId);

            Assert.IsFalse(SystemUnderTest.Facts.Contains(fact0), 
                "Should not contain this fact after remove.");
        }


    }
}
