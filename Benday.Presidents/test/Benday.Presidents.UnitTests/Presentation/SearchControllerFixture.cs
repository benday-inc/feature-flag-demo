using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benday.Presidents.WebUI.Controllers;
using Benday.Presidents.WebUI.Models;
using System.Collections.Generic;
using Benday.Presidents.Api.Models;
using Benday.Presidents.Api.Interfaces;

namespace Benday.Presidents.UnitTests.Presentation
{
    [TestClass]
    public class SearchControllerFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
            _PresidentServiceInstance = null;
            _FeatureManagerInstance = null;
        }

        private SearchController _SystemUnderTest;

        private SearchController SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new SearchController(
                        PresidentServiceInstance,
                        FeatureManagerInstance);
                }

                return _SystemUnderTest;
            }
        }

        private MockFeatureManager _FeatureManagerInstance;
        public MockFeatureManager FeatureManagerInstance
        {
            get
            {
                if (_FeatureManagerInstance == null)
                {
                    _FeatureManagerInstance = new MockFeatureManager();

                    _FeatureManagerInstance.Search = true;
                }

                return _FeatureManagerInstance;
            }
        }

        private MockPresidentService _PresidentServiceInstance;
        public MockPresidentService PresidentServiceInstance
        {
            get
            {
                if (_PresidentServiceInstance == null)
                {
                    _PresidentServiceInstance = new MockPresidentService();
                }

                return _PresidentServiceInstance;
            }
        }

        [TestMethod]
        public void GivenSearchFeatureIsDisabledWhenIndexGetIsCalledThenHttpNotFound()
        {
            FeatureManagerInstance.Search = false;

            UnitTestUtility.AssertIsHttpNotFound(
                SystemUnderTest.Index());
        }

        [TestMethod]
        public void GivenSearchFeatureIsDisabledWhenIndexPostIsCalledThenHttpNotFound()
        {
            FeatureManagerInstance.Search = false;

            UnitTestUtility.AssertIsHttpNotFound(
                SystemUnderTest.Index(new SearchViewModel()));
        }

        [TestMethod]
        public void WhenIndexIsCalledSearchResultsIsBlankAndFirstNameLastNameValuesAreEmpty()
        {
            SearchViewModel actual =
                UnitTestUtility.GetModel<SearchViewModel>(
                    SystemUnderTest.Index());

            Assert.IsNotNull(actual, "Model was null.");

            Assert.IsNotNull(actual.Results, "Search results was null.");
            Assert.AreEqual<int>(0, actual.Results.Count, "Result count should be zero.");
            Assert.AreEqual<string>(String.Empty, actual.FirstName, "FirstName should be empty.");
            Assert.AreEqual<string>(String.Empty, actual.LastName, "LastName should be empty.");
        }

        [TestMethod]
        public void GivenAPopulatedSearchWhenIndexIsCalledThenSearchCriteriaIsOnTheReturnedModel()
        {
            PopulateSearchResults();

            var search = new SearchViewModel();

            string expectedFirstName = "expected fn";
            string expectedLastName = "expected ln";

            search.FirstName = expectedFirstName;
            search.LastName = expectedLastName;

            SearchViewModel actual =
                UnitTestUtility.GetModel<SearchViewModel>(
                    SystemUnderTest.Index(search));

            Assert.AreNotSame(search, actual,
                "This test assumes that a new instance of model is returned from the call to Index().");

            Assert.AreEqual<string>(expectedFirstName, actual.FirstName, "FirstName");
            Assert.AreEqual<string>(expectedLastName, actual.LastName, "LastName");
        }

        [TestMethod]
        public void GivenAPopulatedSearchWhenIndexIsCalledSearchResultsAreReturned()
        {
            PopulateSearchResults();

            var search = new SearchViewModel();

            string expectedFirstName = "expected fn";
            string expectedLastName = "expected ln";

            search.FirstName = expectedFirstName;
            search.LastName = expectedLastName;

            SearchViewModel actual =
                UnitTestUtility.GetModel<SearchViewModel>(
                    SystemUnderTest.Index(search));

            Assert.AreNotEqual<int>(0, actual.Results.Count, "Result count was wrong.");
        }

        [TestMethod]
        public void GivenStateSearchIsOffWhenIndexIsCalledThenSearchResultsIgnoreState()
        {
            FeatureManagerInstance.SearchByBirthDeathState = false;

            PopulateSearchResults();

            var search = new SearchViewModel();

            search.FirstName = String.Empty;
            search.LastName = String.Empty;
            search.BirthState = "Virginia";
            search.DeathState = "Virginia";

            SearchViewModel actual =
                UnitTestUtility.GetModel<SearchViewModel>(
                    SystemUnderTest.Index(search));

            // there should be two search results if state info is ignored
            Assert.AreEqual<int>(2, actual.Results.Count, "Result count was wrong.");
        }

        [TestMethod]
        public void GivenStateSearchIsOnWhenIndexIsCalledThenSearchResultsAreFilteredByState()
        {
            FeatureManagerInstance.SearchByBirthDeathState = true;

            PopulateSearchResultsForStateSearchAndNullStandardSearch();

            var search = new SearchViewModel();

            search.FirstName = String.Empty;
            search.LastName = String.Empty;
            search.BirthState = "Virginia";
            search.DeathState = "Virginia";

            SearchViewModel actual =
                UnitTestUtility.GetModel<SearchViewModel>(
                    SystemUnderTest.Index(search));

            Assert.AreEqual<int>(1, actual.Results.Count, "Result count was wrong.");
        }

        private void PopulateSearchResults()
        {
            PresidentServiceInstance.SearchReturnValue = new List<President>();

            PresidentServiceInstance.SearchReturnValue.Add(
                            UnitTestUtility.GetGroverClevelandAsPresident(true)
                            );

            PresidentServiceInstance.SearchReturnValue.Add(
                UnitTestUtility.GetThomasJeffersonAsPresident(true)
                );
        }

        private void PopulateSearchResultsForStateSearchAndNullStandardSearch()
        {
            PresidentServiceInstance.SearchReturnValue = new List<President>();
            PresidentServiceInstance.SearchReturnValueForStateSearch = new List<President>();

            PresidentServiceInstance.SearchReturnValueForStateSearch.Add(
                UnitTestUtility.GetThomasJeffersonAsPresident(true)
                );
        }

    }
}
