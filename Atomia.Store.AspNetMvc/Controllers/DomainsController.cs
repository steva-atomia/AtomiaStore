using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Atomia.Store.AspNetMvc.Filters;

namespace Atomia.Store.AspNetMvc.Controllers
{
    /// <summary>
    /// Domain search page and JSON API.
    /// </summary>
    public sealed class DomainsController : Controller
    {
        private readonly IDomainsProvider domainProvider = DependencyResolver.Current.GetService<IDomainsProvider>();

        /// <summary>
        /// Domain search page, part of order flow.
        /// </summary>
        [OrderFlowFilter]
        [HttpGet]
        public ActionResult Index(DomainQueryModel prefilled)
        {
            var model = DependencyResolver.Current.GetService<DomainsViewModel>();

            if (ModelState.IsValid) 
            {
                // If query string 'query' is a valid domain search, prefill the search box, 
                // otherwise we silently ignore and let user continue searching manually.
                model.SearchQuery = prefilled.Query;
            }

            return View(model);
        }

        /// <summary>
        /// Find domains based on search query.
        /// </summary>
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

        /// <summary>
        /// Check status of domain search started from <see cref="FindDomains"/>
        /// </summary>
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
