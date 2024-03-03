using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benday.Presidents.Api.Services;
using Benday.Presidents.Api.Models;
using Benday.Presidents.Api;
using Benday.Presidents.Api.DataAccess;

namespace Benday.Presidents.UnitTests.Services
{
    [TestClass]
    public class PersonToPresidentAdapterFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }
        
        private PersonToPresidentAdapter _SystemUnderTest;
        public PersonToPresidentAdapter SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new PersonToPresidentAdapter();
                }

                return _SystemUnderTest;
            }
        }

        [TestMethod]
        public void AdaptPersonToPresident_OneTerm()
        {
            var fromValue = UnitTestUtility.GetThomasJeffersonAsPerson();
            var toValue = new President();

            SystemUnderTest.Adapt(fromValue, toValue);

            UnitTestUtility.AssertAreEqual(fromValue, toValue);
        }

        [TestMethod]
        public void AdaptPersonToPresident_TwoTerms()
        {
            var fromValue = UnitTestUtility.GetGroverClevelandAsPerson();
            var toValue = new President();

            SystemUnderTest.Adapt(fromValue, toValue);

            UnitTestUtility.AssertAreEqual(fromValue, toValue);
        }

        [TestMethod]
        public void GivenASavedPersonWhenAdaptPersonToPresident_TwoTerms()
        {
            var fromValue = UnitTestUtility.GetGroverClevelandAsPerson(true);
            var toValue = new President();

            SystemUnderTest.Adapt(fromValue, toValue);

            UnitTestUtility.AssertAreEqual(fromValue, toValue);
        }

        [TestMethod]
        public void AdaptPresidentToPerson_OneTerm()
        {
            var fromValue = UnitTestUtility.GetThomasJeffersonAsPresident();
            var toValue = new Person();

            SystemUnderTest.Adapt(fromValue, toValue);

            UnitTestUtility.AssertAreEqual(fromValue, toValue);
        }

        [TestMethod]
        public void AdaptPresidentToPerson_TwoTerms()
        {
            var fromValue = UnitTestUtility.GetGroverClevelandAsPresident();
            var toValue = new Person();

            SystemUnderTest.Adapt(fromValue, toValue);

            UnitTestUtility.AssertAreEqual(fromValue, toValue);
        }

        [TestMethod]
        public void AdaptPresidentToPerson_GivenAnEmptyPersonWhenPresidentHasDeletedTermsThenDeletedTermsAreSkipped()
        {
            var fromValue = UnitTestUtility.GetGroverClevelandAsPresident(true);
            var fromValueTermThatsMarkedForDelete = fromValue.Terms[0];
            fromValueTermThatsMarkedForDelete.IsDeleted = true;

            var toValue = new Person();

            SystemUnderTest.Adapt(fromValue, toValue);

            fromValue.Terms.Remove(fromValueTermThatsMarkedForDelete);
            UnitTestUtility.AssertAreEqual(fromValue, toValue);
        }

        [TestMethod]
        public void AdaptPresidentToPerson_GivenANonEmptyPersonWhenPresidentHasDeletedTermsThenDeletedTermsAreSkipped()
        {
            // arrange
            var fromValue = UnitTestUtility.GetGroverClevelandAsPresident(true);
            var toValue = new Person();
            SystemUnderTest.Adapt(fromValue, toValue);
            var fromValueTermThatsMarkedForDelete = fromValue.Terms[0];
            Assert.AreNotEqual<int>(0, 
                fromValueTermThatsMarkedForDelete.Id, 
                "Term that marked for delete should have a simulated saved identity value.");
            fromValueTermThatsMarkedForDelete.IsDeleted = true;

            // act
            SystemUnderTest.Adapt(fromValue, toValue);

            // assert
            fromValue.Terms.Remove(fromValueTermThatsMarkedForDelete);
            UnitTestUtility.AssertAreEqual(fromValue, toValue);
        }

        [TestMethod]
        public void GivenAnUnsavedPresidentWhenAdaptIsCalledTwiceThenNoDuplicatesAreCreated()
        {
            var fromValue = UnitTestUtility.GetGroverClevelandAsPresident();
            var toValue = new Person();

            SystemUnderTest.Adapt(fromValue, toValue);
            SystemUnderTest.Adapt(fromValue, toValue);

            UnitTestUtility.AssertAreEqual(fromValue, toValue);
        }

        [TestMethod]
        public void GivenASavedPresidentWhenAdaptIsCalledTwiceThenNoDuplicatesAreCreated()
        {
            var fromValue = UnitTestUtility.GetGroverClevelandAsPresident(true);
            var toValue = new Person();

            SystemUnderTest.Adapt(fromValue, toValue);
            SystemUnderTest.Adapt(fromValue, toValue);

            UnitTestUtility.AssertAreEqual(fromValue, toValue);
        }
    }
}
