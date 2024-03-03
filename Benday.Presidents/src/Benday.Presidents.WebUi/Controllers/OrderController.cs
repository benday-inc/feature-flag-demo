using Benday.Presidents.Api.Services;
using Benday.Presidents.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Benday.Presidents.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Benday.Presidents.WebUI.Controllers
{
    public class OrderController : Controller
    {
        private IOrderService _Service;

        public OrderController(IOrderService service)
        {
            if (service == null)
                throw new ArgumentNullException("service", "service is null.");

            _Service = service;
        }

        // get order by id
        public ActionResult Index(int? id)
        {
            if (id == null || id.HasValue == false)
            {
                return new BadRequestResult();
            }

            var fromValue = _Service.GetById(id.Value);

            if (fromValue == null)
            {
                return NotFound();
            }

            var model = new OrderInfoViewModel();

            Adapt(fromValue, model);

            if (User.IsInRole("Administrator") == true)
            {
                model.IsInternalOrderInformationVisible = true;
            }
            else
            {
                model.IsInternalOrderInformationVisible = false;

                model.ApprovedBy = String.Empty;
                model.InternalOrderNumber = String.Empty;
                model.ProfitAmount = 0d;
            }

            return View(model);
        }

        private void Adapt(Order fromValue, OrderInfoViewModel toValue)
        {
            toValue.Id = fromValue.Id;
            toValue.ShipToAddress = fromValue.ShipToAddress;
            toValue.ShipToCity = fromValue.ShipToCity;
            toValue.ShipToName = fromValue.ShipToName;
            toValue.ShipToPostalCode = fromValue.ShipToPostalCode;
            toValue.ShipToState = fromValue.ShipToState;

            toValue.ApprovedBy = fromValue.ApprovedBy;
            toValue.InternalOrderNumber = fromValue.InternalOrderNumber;
            toValue.ProfitAmount = fromValue.ProfitAmount;
        }
    }
}