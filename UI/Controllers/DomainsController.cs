using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public sealed class DomainsController : Controller
    {
        private readonly IModelProvider modelProvider;
        private readonly IDomainSearchService domainSearchService;

        public DomainsController(IModelProvider modelProvider, IDomainSearchService domainSearchService)
        {
            this.modelProvider = modelProvider;
            this.domainSearchService = domainSearchService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = modelProvider.Create<DomainsViewModel>();
            
            return View(model);
        }

        [HttpGet]
        public JsonResult FindDomains(DomainSearchQuery searchQuery)
        {
            if (ModelState.IsValid)
            {
                var searchResults = domainSearchService.FindDomains(searchQuery);

                return JsonEnvelope.Success(searchResults);
            }

            return JsonEnvelope.Fail(ModelState);
        }
    }
}
