using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Atomia.Store.Core;
using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.AspNetMvc.Providers;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public class DomainsController : Controller
    {
        private readonly DomainsProvider domainSearchProvider;

        public DomainsController()
        {
            domainSearchProvider = DependencyResolver.Current.GetService<DomainsProvider>();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = DependencyResolver.Current.GetService<DomainsViewModel>();

            return View(model);
        }

        [HttpGet]
        public JsonResult FindDomains(DomainQueryModel searchQuery)
        {
            if (ModelState.IsValid)
            {
                var searchResults = domainSearchProvider.GetProducts(new ProductSearchQuery
                {
                    Terms = new List<SearchTerm>
                    {
                        new SearchTerm 
                        {
                            Key = "query",
                            Value = searchQuery.Query
                        }
                    }
                }).Select(r => new ProductModel(r)).ToList();

                return JsonEnvelope.Success(searchResults);
            }

            return JsonEnvelope.Fail(ModelState);
        }
    }
}
