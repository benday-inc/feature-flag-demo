using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benday.Presidents.Api.Models;
using Benday.Presidents.WebUI.Controllers;
using Benday.Presidents.WebUI.Models;

namespace Benday.Presidents.UnitTests.Presentation
{
    [TestClass]
    public class OrderControllerFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private OrderController _SystemUnderTest;

        private OrderController SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new OrderController(
                        OrderServiceInstance);
                }

                return _SystemUnderTest;
            }
        }

        private OrderServiceMock _OrderServiceInstance;
        public OrderServiceMock OrderServiceInstance
        {
            get
            {
                if (_OrderServiceInstance == null)
                {
                    _OrderServiceInstance = new OrderServiceMock();
                }

                return _OrderServiceInstance;
            }
        }
        
        [TestMethod]
        public void WhenLoggedInAsUserThenProfitDetailsAreHiddenAndUnpopulated()
        {
            // arrange
            var roles = new string[] { "User" };

            ControllerSecurityUtility.SetControllerContext(
                SystemUnderTest, "fake user",
                roles);

            var expected = GetTestOrder();

            OrderServiceInstance.GetByIdReturnValue = expected;

            // act
            var actual = UnitTestUtility.GetModel<OrderInfoViewModel>(
                SystemUnderTest.Index(1234)
                );

            // assert
            Assert.IsFalse(actual.IsInternalOrderInformationVisible, "Internal order information should be invisible");
            Assert.AreEqual<string>(String.Empty, actual.ApprovedBy, "ApprovedBy");
            Assert.AreEqual<string>(String.Empty, actual.InternalOrderNumber, "InternalOrderNumber");
            Assert.AreEqual<double>(0d, actual.ProfitAmount, "ProfitAmount");

            Assert.AreEqual<int>(expected.Id, actual.Id, "Id");
            Assert.AreEqual<string>(expected.ShipToAddress, actual.ShipToAddress, "ShipToAddress");
            Assert.AreEqual<string>(expected.ShipToCity, actual.ShipToCity, "ShipToCity");
            Assert.AreEqual<string>(expected.ShipToName, actual.ShipToName, "ShipToName");
            Assert.AreEqual<string>(expected.ShipToPostalCode, actual.ShipToPostalCode, "ShipToPostalCode");
            Assert.AreEqual<string>(expected.ShipToState, actual.ShipToState, "ShipToState");            
        }

        [TestMethod]
        public void WhenLoggedInAsAdministratorThenProfitDetailsAreVisibleAndPopulated()
        {
            // arrange
            var roles = new string[] { "Administrator" };

            ControllerSecurityUtility.SetControllerContext(
                SystemUnderTest, "fake user",
                roles);

            var expected = GetTestOrder();

            OrderServiceInstance.GetByIdReturnValue = expected;

            // act
            var actual = UnitTestUtility.GetModel<OrderInfoViewModel>(
                SystemUnderTest.Index(1234)
                );

            // assert
            Assert.IsTrue(actual.IsInternalOrderInformationVisible, "Internal order information should be visible");
            Assert.AreEqual<string>(expected.ApprovedBy, actual.ApprovedBy, "ApprovedBy");
            Assert.AreEqual<string>(expected.InternalOrderNumber, actual.InternalOrderNumber, "InternalOrderNumber");
            Assert.AreEqual<double>(expected.ProfitAmount, actual.ProfitAmount, "ProfitAmount");

            Assert.AreEqual<int>(expected.Id, actual.Id, "Id");
            Assert.AreEqual<string>(expected.ShipToAddress, actual.ShipToAddress, "ShipToAddress");
            Assert.AreEqual<string>(expected.ShipToCity, actual.ShipToCity, "ShipToCity");
            Assert.AreEqual<string>(expected.ShipToName, actual.ShipToName, "ShipToName");
            Assert.AreEqual<string>(expected.ShipToPostalCode, actual.ShipToPostalCode, "ShipToPostalCode");
            Assert.AreEqual<string>(expected.ShipToState, actual.ShipToState, "ShipToState");
        }
                
        private Order GetTestOrder()
        {
            Order returnValue = new Order();

            returnValue.ApprovedBy = "Skip Rosenwinkle";
            returnValue.Id = 300;
            returnValue.InternalOrderNumber = "1F82G288";
            returnValue.ProfitAmount = 450.27;
            returnValue.ShipToAddress = "1 Main St";
            returnValue.ShipToCity = "Fake Town";
            returnValue.ShipToName = "Fake Name";
            returnValue.ShipToPostalCode = "12345";
            returnValue.ShipToState = "MA";

            return returnValue;
        }
    }
}
