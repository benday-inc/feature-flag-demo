using Benday.Presidents.Api.Services;
using Benday.Presidents.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Benday.Presidents.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Benday.Presidents.UnitTests.Presentation
{
    [TestClass]
    public class PresidentControllerFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
            _PresidentServiceInstance = null;
        }

        private PresidentController _SystemUnderTest;

        private PresidentController SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new PresidentController(
                        PresidentServiceInstance
                        );
                }

                return _SystemUnderTest;
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
        public void WhenIndexIsCalledThenAllPresidentsAreReturned()
        {
            PresidentServiceInstance.GetPresidentsReturnValue.Add(
                UnitTestUtility.GetGroverClevelandAsPresident(true)
                );

            PresidentServiceInstance.GetPresidentsReturnValue.Add(
                UnitTestUtility.GetThomasJeffersonAsPresident(true)
                );

            var model = 
                UnitTestUtility.GetModel<IList<President>>(
                    SystemUnderTest.Index());

            Assert.IsNotNull(model, "Model was null.");

            Assert.AreNotEqual<int>(0, model.Count, "Model count was wrong.");
        }

        [TestMethod]
        public void WhenDetailsIsCalledForValidPresidentIdThenPresidentIsReturned()
        {
            var expected = UnitTestUtility.GetGroverClevelandAsPresident(true);

            PresidentServiceInstance.GetPresidentByIdReturnValue =
                expected;

            var model =
                UnitTestUtility.GetModel<President>(
                    SystemUnderTest.Details(1234));

            Assert.IsNotNull(model, "Model was null.");

            Assert.AreSame(expected, model);
        }

        [TestMethod]
        public void WhenDetailsIsCalledForUnknownPresidentIdThenHttpNotFoundReturned()
        {
            PresidentServiceInstance.GetPresidentByIdReturnValue = null;

            UnitTestUtility.AssertIsHttpNotFound(
                SystemUnderTest.Details(1234));
        }

        [TestMethod]
        public void WhenEditIsCalledForLoadForValidPresidentIdThenPresidentIsReturned()
        {
            var expected = UnitTestUtility.GetGroverClevelandAsPresident(true);

            PresidentServiceInstance.GetPresidentByIdReturnValue =
                expected;

            var model =
                UnitTestUtility.GetModel<President>(
                    SystemUnderTest.Edit(1234));

            Assert.IsNotNull(model, "Model was null.");

            Assert.AreSame(expected, model);
        }

        [TestMethod]
        public void WhenEditIsCalledForLoadForCreateNewPresidentThenThereIsAnEmptyPresidentRow()
        {
            var model =
                UnitTestUtility.GetModel<President>(
                    SystemUnderTest.Edit(0));

            Assert.IsNotNull(model, "Model was null.");

            Assert.AreEqual<int>(1, model.Terms.Count, "Term count should be 1");

            Assert.AreEqual<string>("President", model.Terms[0].Role, "Role was wrong.");
        }

        [TestMethod]
        public void WhenEditIsCalledForSaveThenPresidentIsSaved()
        {
            var saveThis = UnitTestUtility.GetGroverClevelandAsPresident(true);

            PresidentServiceInstance.GetPresidentByIdReturnValue =
                saveThis;

            var result = SystemUnderTest.Edit(saveThis) as RedirectToActionResult;

            Assert.IsNotNull(result);

            Assert.IsNotNull(PresidentServiceInstance.SavePresidentArgumentValue, 
                "Service.Save() was not called with a non-null value.");
        }

        [TestMethod]
        public void WhenEditIsCalledForSaveNewPresidentThenPresidentIsSaved()
        {
            // unsaved
            var saveThis = UnitTestUtility.GetGroverClevelandAsPresident();

            PresidentServiceInstance.GetPresidentByIdReturnValue = null;

            var result = SystemUnderTest.Edit(saveThis);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            Assert.IsNotNull(PresidentServiceInstance.SavePresidentArgumentValue,
                "Service.Save() was not called with a non-null value.");
        }
    }
}
