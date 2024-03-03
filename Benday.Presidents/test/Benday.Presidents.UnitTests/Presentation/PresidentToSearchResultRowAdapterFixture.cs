using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benday.Presidents.WebUI.Models;
using Benday.Presidents.Api.Models;

namespace Benday.Presidents.UnitTests.Presentation
{
    [TestClass]
    public class PresidentToSearchResultRowAdapterFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private PresidentToSearchResultRowAdapter _SystemUnderTest;

        private PresidentToSearchResultRowAdapter SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new PresidentToSearchResultRowAdapter();
                }

                return _SystemUnderTest;
            }
        }

        [TestMethod]
        public void AdaptPresidentToSearchResultRow()
        {
            var actual = new SearchResultRow();

            var fromValue = UnitTestUtility.GetThomasJeffersonAsPresident(true);

            SystemUnderTest.Adapt(fromValue, actual);

            AssertAreEqual(fromValue, actual);
        }

        private void AssertAreEqual(President expected, SearchResultRow actual)
        {
            Assert.AreEqual<string>(expected.FirstName, actual.FirstName, "FirstName");
            Assert.AreEqual<string>(expected.LastName, actual.LastName, "LastName");
            Assert.AreEqual<int>(expected.Id, actual.Id, "Id");
        }


    }
}
