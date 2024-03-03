using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benday.Presidents.Api.Models;

namespace Benday.Presidents.UnitTests.Models
{
    [TestClass]
    public class PresidentFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private President _SystemUnderTest;

        private President SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new President();
                }

                return _SystemUnderTest;
            }
        }

        [TestMethod]
        public void TermsCollectionIsNotNull()
        {
            Assert.IsNotNull(SystemUnderTest.Terms);
        }

        [TestMethod]
        public void FieldsAreInitialized()
        {
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.BirthCity, "BirthCity should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.BirthState, "BirthState should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.DeathCity, "DeathCity should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.DeathState, "DeathState should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.FirstName, "FirstName should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.LastName, "LastName should be empty.");
        }


    }
}
