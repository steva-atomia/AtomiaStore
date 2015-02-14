using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Atomia.Store.Core;
using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public class DomainController : Controller
    {
        private readonly IDomainsProvider domainProvider;

        public DomainController()
        {
            this.domainProvider = DependencyResolver.Current.GetService<IDomainsProvider>();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = DependencyResolver.Current.GetService<DomainViewModel>();

            return View(model);
        }

        [HttpPost]
        public JsonResult FindDomains(DomainQueryModel searchQuery)
        {
            if (ModelState.IsValid)
            {
                var searchTerms = new List<string> { searchQuery.Query };
                var searchResults = domainProvider.FindDomains(searchTerms)
                    .Select(r => new DomainResultModel(r)).ToList();

                return JsonEnvelope.Success(searchResults);
            }

            return JsonEnvelope.Fail(ModelState);
        }

        [HttpPost]
        public JsonResult CheckStatus(int domainSearchId)
        {
            if (ModelState.IsValid)
            {
                var searchResults = domainProvider.CheckStatus(domainSearchId)
                    .Select(r => new DomainResultModel(r)).ToList();
            }

            return JsonEnvelope.Fail(ModelState);
        }
    }
}
