using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Benday.Presidents.Api.Models;
using Benday.Presidents.Api.Services;

namespace Benday.Presidents.UnitTests.Services
{
    [TestClass]
    public class PresidentValidatorStrategyFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private PresidentValidatorStrategy _SystemUnderTest;

        private PresidentValidatorStrategy SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new PresidentValidatorStrategy();
                }

                return _SystemUnderTest;
            }
        }

        [TestMethod]
        public void PresidentMustHaveAValidFirstName()
        {
            President validateThis = 
                UnitTestUtility.GetThomasJeffersonAsPresident();

            validateThis.FirstName = null;

            var actual = SystemUnderTest.IsValid(validateThis);

            var expected = false;

            Assert.AreEqual<bool>(expected, actual, "IsValid() returned unexpected result.");
        }

        [TestMethod]
        public void NullPresidentIsInvalid()
        {
            President validateThis = null;

            var actual = SystemUnderTest.IsValid(validateThis);

            var expected = false;

            Assert.AreEqual<bool>(expected, actual, "IsValid() returned unexpected result.");
        }

        [TestMethod]
        public void PresidentMustHaveAValidLastName()
        {
            President validateThis =
                UnitTestUtility.GetThomasJeffersonAsPresident();

            validateThis.LastName = null;

            var actual = SystemUnderTest.IsValid(validateThis);

            var expected = false;

            Assert.AreEqual<bool>(expected, actual, "IsValid() returned unexpected result.");
        }

        [TestMethod]
        public void PresidentMustHaveAtLeastOneTerm()
        {
            President validateThis =
                UnitTestUtility.GetThomasJeffersonAsPresident();

            validateThis.Terms.Clear();

            var actual = SystemUnderTest.IsValid(validateThis);

            var expected = false;

            Assert.AreEqual<bool>(expected, actual, "IsValid() returned unexpected result.");
        }


        [TestMethod]
        public void PresidentWithFirstNameLastNameAndTermIsValid()
        {
            President validateThis =
                UnitTestUtility.GetThomasJeffersonAsPresident();

            var actual = SystemUnderTest.IsValid(validateThis);

            var expected = true;

            Assert.AreEqual<bool>(expected, actual, "IsValid() returned unexpected result.");
        }
    }
}
