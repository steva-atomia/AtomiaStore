﻿using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Atomia.Store.AspNetMvc.Filters;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public sealed class DomainsController : Controller
    {
        private readonly IDomainsProvider domainProvider = DependencyResolver.Current.GetService<IDomainsProvider>();

        [OrderFlowFilter]
        [HttpGet]
        public ActionResult Index()
        {
            var model = DependencyResolver.Current.GetService<DomainsViewModel>();

            return View(model);
        }

        [HttpPost]
        public JsonResult FindDomains(DomainQueryModel searchQuery)
        {
            if (ModelState.IsValid)
            {
                var searchTerms = new List<string> { searchQuery.Query };
                var domainSearchData = domainProvider.FindDomains(searchTerms);

                return JsonEnvelope.Success(new
                {
                    DomainSearchId = domainSearchData.DomainSearchId,
                    FinishSearch = domainSearchData.FinishSearch,
                    Results = domainSearchData.Results.Select(r => new DomainResultModel(r)).ToList()
                });
            }

            return JsonEnvelope.Fail(ModelState);
        }

        [HttpPost]
        public JsonResult CheckStatus(int domainSearchId)
        {
            if (ModelState.IsValid)
            {
                var domainSearchData = domainProvider.CheckStatus(domainSearchId);

                return JsonEnvelope.Success(new
                {
                    DomainSearchId = domainSearchData.DomainSearchId,
                    FinishSearch = domainSearchData.FinishSearch,
                    Results = domainSearchData.Results.Select(r => new DomainResultModel(r)).ToList()
                });
            }

            return JsonEnvelope.Fail(ModelState);
        }
    }
}