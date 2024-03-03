using Benday.Presidents.Api.Services;
using Benday.Presidents.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Benday.Presidents.Api.Models;
using Benday.Presidents.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Benday.Presidents.WebUI.Controllers
{
    public class SearchController : Controller
    {
        private IPresidentService _Service;
        private IFeatureManager _FeatureManager;

        public SearchController(IPresidentService service, 
            IFeatureManager featureManager)
        {
            if (service == null)
                throw new ArgumentNullException("service", "service is null.");
            if (featureManager == null)
                throw new ArgumentNullException("featureManager", "featureManager is null.");

            _Service = service;
            _FeatureManager = featureManager;
        }

        // GET: Search
        public ActionResult Index()
        {
            if (_FeatureManager.Search == false)
            {
                return NotFound();
            }

            var model = new SearchViewModel();

            if (_FeatureManager.SearchByBirthDeathState == true)
            {
                return View("IndexStateSearch", model);
            }
            else
            {
                return View(model);
            }            
        }

        [HttpPost]
        public ActionResult Index(SearchViewModel model)
        {
            if (model == null)
            {
                throw new InvalidOperationException("Argument cannot be null.");
            }

            if (_FeatureManager.Search == false)
            {
                return NotFound();
            }

            // var results = _Service.Search(model.FirstName, model.LastName);

            IList<President> results = null;

            if (_FeatureManager.SearchByBirthDeathState == true)
            {
                results = _Service.Search(
                    model.FirstName, model.LastName,
                    model.BirthState, model.DeathState);
            }
            else
            {
                results = _Service.Search(
                    model.FirstName, model.LastName);
            }

            var modelToReturn = new SearchViewModel();

            modelToReturn.FirstName = model.FirstName;
            modelToReturn.LastName = model.LastName;

            if (results != null)
            {
                Adapt(results, modelToReturn.Results);
            }

            if (_FeatureManager.SearchByBirthDeathState == true)
            {
                return View("IndexStateSearch", modelToReturn);
            }
            else
            {
                return View(modelToReturn);
            }
        }

        private void Adapt(IList<President> fromValues, List<SearchResultRow> toValues)
        {
            if (fromValues == null)
                throw new ArgumentNullException("fromValues", "fromValues is null.");
            if (toValues == null)
                throw new ArgumentNullException("toValues", "toValues is null.");

            var adapter = new PresidentToSearchResultRowAdapter();

            SearchResultRow toValue;

            foreach (var fromValue in fromValues)
            {
                toValue = new SearchResultRow();

                adapter.Adapt(fromValue, toValue);

                toValues.Add(toValue);
            }
        }
    }
}