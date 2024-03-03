using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benday.Presidents.Api.DataAccess;
using Benday.Presidents.Api.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
// using System.Data.Entity;

namespace Benday.Presidents.IntegrationTests
{

    [TestClass]
    public class PresidentsDbContextEntityFrameworkFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private PresidentsDbContext _SystemUnderTest;
        public PresidentsDbContext SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new PresidentsDesignTimeDbContextFactory().Create();

                    _SystemUnderTest.Database.EnsureCreated();
                }

                return _SystemUnderTest;
            }
        }

        [TestMethod]
        [TestCategory("Integration.Sql")]
        public void EntityFramework_SavePerson_Simple()
        {
            var person = new Person();

            person.FirstName = "test_fn";
            person.LastName = "test_ln";

            SystemUnderTest.Persons.Add(person);

            SystemUnderTest.SaveChanges();

            Assert.AreNotEqual<int>(0, person.Id, "Person id should not be 0.");
        }

        [TestMethod]
        [TestCategory("Integration.Sql")]
        public void EntityFramework_SavePersonWithRelationship_CreatesRelationshipRecord()
        {
            var fromPerson = new Person();

            fromPerson.FirstName = "from_fn_1";
            fromPerson.LastName = "from_ln_1";

            var toPerson = new Person();

            toPerson.FirstName = "to_fn_1";
            toPerson.LastName = "to_ln_1";

            SystemUnderTest.Persons.Add(fromPerson);
            SystemUnderTest.Persons.Add(toPerson);

            fromPerson.AddRelationship("Vice President", toPerson);

            SystemUnderTest.SaveChanges();

            Assert.AreNotEqual<int>(0, fromPerson.Id, "From Person id should not be 0.");
            Assert.AreNotEqual<int>(0, toPerson.Id, "To Person id should not be 0.");
            Assert.AreNotEqual<int>(0, fromPerson.Relationships[0].Id, "Relationship id should not be 0.");
        }

        [TestMethod]
        [TestCategory("Integration.Sql")]
        public void EntityFramework_LoadPersonWithRelationship_UsingFreshDbContext_FieldsArePopulated()
        {
            var expected = SavePersonWithRelationship();

            _SystemUnderTest = null;

            var actual = (
                from temp in SystemUnderTest.Persons.Include(
                    x => x.Relationships)
                    .ThenInclude(r => r.ToPerson)
                where temp.Id == expected.Id
                select temp
                ).FirstOrDefault();

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Relationships);
            Assert.AreNotEqual<int>(0, actual.Relationships.Count, "Should be a relationship");

            var actualRelationship = actual.Relationships[0];

            Assert.IsNotNull(actualRelationship.FromPerson, "FromPerson was null.");
            Assert.IsNotNull(actualRelationship.ToPerson, "ToPerson was null.");
        }

        private Person SavePersonWithRelationship()
        {
            var fromPerson = new Person();

            fromPerson.FirstName = "from_fn_1";
            fromPerson.LastName = "from_ln_1";

            var toPerson = new Person();

            toPerson.FirstName = "to_fn_1";
            toPerson.LastName = "to_ln_1";

            SystemUnderTest.Persons.Add(fromPerson);
            SystemUnderTest.Persons.Add(toPerson);

            fromPerson.AddRelationship("Vice President", toPerson);

            SystemUnderTest.SaveChanges();

            return fromPerson;
        }

        [TestMethod]
        [TestCategory("Integration.Sql")]
        public void EntityFramework_SavePersonWithFact_SingleFact()
        {
            var person = new Person();

            person.FirstName = "from_fn_1";
            person.LastName = "from_ln_1";

            var expectedFactType = "Role";
            var expectedFactValue = "US Senator";

            person.AddFact(expectedFactType, expectedFactValue);

            SystemUnderTest.Persons.Add(person);

            SystemUnderTest.SaveChanges();

            Assert.AreNotEqual<int>(0, person.Id, "From Person id should not be 0.");
            Assert.AreNotEqual<int>(0, person.Facts[0].Id, "Person fact id should not be 0.");
        }

        [TestMethod]
        [TestCategory("Integration.Sql")]
        public void EntityFramework_SavePersonWithFact_MultipleFacts()
        {
            var person = new Person();

            person.FirstName = "from_fn_1";
            person.LastName = "from_ln_1";

            person.AddFact("Role", "US Senator");
            person.AddFact("Birth Date", new DateTime(1928, 3, 21));

            SystemUnderTest.Persons.Add(person);

            SystemUnderTest.SaveChanges();

            Assert.AreNotEqual<int>(0, person.Id, "From Person id should not be 0.");
            Assert.AreNotEqual<int>(0, person.Facts[0].Id, "Person fact #0 id should not be 0.");
            Assert.AreNotEqual<int>(0, person.Facts[1].Id, "Person fact #1 id should not be 0.");
        }

        [TestMethod]
        [TestCategory("Integration.Sql")]
        public void EntityFramework_LoadPersonWithFacts_UsingFreshDbContext_FieldsArePopulated()
        {
            var expected = new Person();

            expected.FirstName = "from_fn_1";
            expected.LastName = "from_ln_1";

            expected.AddFact("Role", "US Senator");
            expected.AddFact("Birth Date", new DateTime(1928, 3, 21));

            SystemUnderTest.Persons.Add(expected);

            SystemUnderTest.SaveChanges();

            _SystemUnderTest = null;

            var actual = (
                from temp in SystemUnderTest.Persons.Include(x => x.Facts)
                where temp.Id == expected.Id
                select temp
                ).FirstOrDefault();

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Facts);
            Assert.AreEqual<int>(2, actual.Facts.Count, "Fact count was wrong");

            foreach (var actualFact in actual.Facts)
            {
                Assert.IsNotNull(actualFact.Person, "Fact.Person was null.");
            }
        }
    }
}
