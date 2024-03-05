using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benday.Presidents.Api.DataAccess;
using System.Linq;
using Benday.Presidents.Api.Models;
// using Benday.Presidents.Tests.Common;
using System.Collections.Generic;
using System.Xml.Linq;
using Benday.Presidents.Api;
using Benday.Presidents.Api.DataAccess.SqlServer;
using Benday.Presidents.Tests.Common;

namespace Benday.Presidents.IntegrationTests
{
    [TestClass]
    public class SqlEntityFrameworkPersonRepositoryFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private SqlEntityFrameworkPersonRepository _SystemUnderTest;
        public SqlEntityFrameworkPersonRepository SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    var context = 
                        new PresidentsDesignTimeDbContextFactory().Create();

                    context.Database.EnsureCreated();

                    _SystemUnderTest =
                        new SqlEntityFrameworkPersonRepository(
                            context);                    
                }

                return _SystemUnderTest;
            }
        }

        private Person CreateTestPerson()
        {
            var temp = new Person();

            temp.FirstName = UnitTestDataUtility.GetUniqueValue("fn_");
            temp.LastName = UnitTestDataUtility.GetUniqueValue("ln_");

            return temp;
        }

        [TestCategory("Integration.Sql")]
        [TestMethod]
        public void SqlEntityFrameworkPersonRepository_SavePerson_New()
        {
            var actual = CreateTestPerson();

            SystemUnderTest.Save(actual);

            Assert.AreNotEqual<int>(0, actual.Id, "Was not saved.");
        }

        [TestCategory("Integration.Sql")]
        [TestMethod]
        public void SqlEntityFrameworkPersonRepository_SavePerson_Modify()
        {
            var actual = CreateTestPerson();

            SystemUnderTest.Save(actual);

            actual.FirstName = "modified_fn";
            actual.LastName = "modified_ln";

            SystemUnderTest.Save(actual);

            ReloadWithNewRepositoryAndAssert(actual);
        }

        [TestCategory("Integration.Sql")]
        [TestMethod]
        public void SqlEntityFrameworkPersonRepository_SavePerson_RemoveExistingFact()
        {
            // arrange
            var actual = CreateTestPerson();

            actual.AddFact(PresidentsConstants.President, new DateTime(2001, 1, 1), new DateTime(2005, 1, 1));
            actual.AddFact(PresidentsConstants.President, new DateTime(2007, 1, 1), new DateTime(2010, 1, 1));

            SystemUnderTest.Save(actual);

            // act
            actual.Facts.Remove(actual.Facts[0]);

            SystemUnderTest.Save(actual);

            ReloadWithNewRepositoryAndAssert(actual);
        }

        [TestCategory("Integration.Sql")]
        [TestMethod]
        public void SqlEntityFrameworkPersonRepository_DeletePerson()
        {
            var actual = CreateTestPerson();
            SystemUnderTest.Save(actual);

            SystemUnderTest.Delete(actual);

            ReloadWithNewRepositoryAndAssertDoesNotExist(actual);
        }

        [TestMethod]
        [TestCategory("Integration.Sql")]
        public void SqlEntityFrameworkPersonRepository_SavePerson_WithRelationships()
        {
            var person1 = CreateTestPerson();
            var person2 = CreateTestPerson();

            person1.AddRelationship("Vice President", person2);

            SystemUnderTest.Save(person1);

            ReloadWithNewRepositoryAndAssertRelationshipsArePopulated(person1);
        }

        [TestMethod]
        [TestCategory("Integration.Sql")]
        public void SqlEntityFrameworkPersonRepository_SavePerson_DeleteExistingRelationship()
        {
            // arrange
            var person1 = CreateTestPerson();
            var person2 = CreateTestPerson();

            person1.AddRelationship("Vice President", person2);

            SystemUnderTest.Save(person1);

            // act
            person1.Relationships.RemoveAt(0);

            SystemUnderTest.Save(person1);

            // assert
            ReloadWithNewRepositoryAndAssertRelationshipsAreEmpty(person1);
        }

        [TestCategory("Integration.Sql")]
        [TestMethod]
        public void SqlEntityFrameworkPersonRepository_SavePerson_ModifyDetachedInstance()
        {
            // arrange
            var original = CreateTestPerson();

            SystemUnderTest.Save(original);

            _SystemUnderTest = null;

            var toBeModified = new Person();

            toBeModified.Id = original.Id;
            toBeModified.FirstName = "modified_fn";
            toBeModified.LastName = "modified_ln";

            // act
            SystemUnderTest.Save(toBeModified);

            // assert
            ReloadWithNewRepositoryAndAssert(toBeModified);
        }

        private void ReloadWithNewRepositoryAndAssert(Person expected)
        {
            _SystemUnderTest = null;

            var actual = SystemUnderTest.GetById(expected.Id);

            Assert.AreEqual<string>(expected.FirstName, actual.FirstName, "FirstName");
            Assert.AreEqual<string>(expected.LastName, actual.LastName, "LastName");

            Assert.AreEqual<int>(expected.Facts.Count, actual.Facts.Count, "Facts.Count");
        }

        private void ReloadWithNewRepositoryAndAssertRelationshipsArePopulated(Person expected)
        {
            _SystemUnderTest = null;

            var actual = SystemUnderTest.GetById(expected.Id);

            Assert.AreEqual<string>(expected.FirstName, actual.FirstName, "FirstName");
            Assert.AreEqual<string>(expected.LastName, actual.LastName, "LastName");

            Assert.IsNotNull(actual.Relationships, "Relationships collection was null.");
            Assert.AreNotEqual<int>(0, actual.Relationships.Count, "Relationship count was wrong.");
            var relationship = actual.Relationships[0];

            Assert.IsNotNull(relationship.FromPerson, "FromPerson was null.");
            Assert.IsNotNull(relationship.ToPerson, "ToPerson was null.");
        }

        private void ReloadWithNewRepositoryAndAssertRelationshipsAreEmpty(Person expected)
        {
            _SystemUnderTest = null;

            var actual = SystemUnderTest.GetById(expected.Id);

            Assert.AreEqual<string>(expected.FirstName, actual.FirstName, "FirstName");
            Assert.AreEqual<string>(expected.LastName, actual.LastName, "LastName");

            Assert.IsNotNull(actual.Relationships, "Relationships collection was null.");
            Assert.AreEqual<int>(0, actual.Relationships.Count, "Relationship count was wrong.");
        }

        private void ReloadWithNewRepositoryAndAssertDoesNotExist(Person expected)
        {
            _SystemUnderTest = null;

            var actual = SystemUnderTest.GetById(expected.Id);

            Assert.IsNull(actual, "Should not exist and this value should be null.");
        }
    }
}
