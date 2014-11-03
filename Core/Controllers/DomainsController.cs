using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atomia.OrderPage.Core.Models;
using Atomia.OrderPage.Core.Services;
using Atomia.OrderPage.Core.Infrastructure;

namespace Atomia.OrderPage.Core.Controllers
{
    public sealed class DomainsController : Controller
    {
        private readonly IModelProvider modelProvider;
        private readonly IDomainSearchService domainSearchService;
        private readonly IDomainStatusService domainStatusService;

        public DomainsController(IModelProvider modelProvider, IDomainSearchService domainSearchService, IDomainStatusService domainStatusService)
        {
            this.modelProvider = modelProvider;
            this.domainSearchService = domainSearchService;
            this.domainStatusService = domainStatusService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            // Simple view that is basically a holder for SearchPartial and SelectPartial

            // Setup OrderFlow 

            return View();
        }

        [ChildActionOnly]
        public PartialViewResult SearchPartial()
        {
            var model = modelProvider.Create<DomainsModel>();

            return PartialView(model);
        }

        [ChildActionOnly]
        public PartialViewResult SelectionPartial()
        {
            var model = modelProvider.Create<DomainsModel>();

            return PartialView(model);
        }

        public JsonResult Search(DomainSearchQuery query)
        {
            if (ModelState.IsValid)
            {
                var result = domainSearchService.FindDomains(query);
            }
            else
            {
                // Error handling
            }

            return new JsonResult();
        }

        public JsonResult Status(DomainStatusQuery query)
        {
            if (ModelState.IsValid)
            {
                var result = domainStatusService.CheckStatus(query);
            }
            else
            {
                // Error handling
            }
            
            return new JsonResult();
        }
    }
}
