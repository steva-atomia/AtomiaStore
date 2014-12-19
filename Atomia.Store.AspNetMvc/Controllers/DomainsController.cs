using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public sealed class DomainsController : Controller
    {
        private readonly IModelProvider modelProvider;
        private readonly IProductsProvider productsProvider;

        public DomainsController(IModelProvider modelProvider, IProductsProvider productsProvider)
        {
            this.modelProvider = modelProvider;
            this.productsProvider = productsProvider;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = modelProvider.Create<DomainsViewModel>();
            
            return View(model);
        }

        [HttpGet]
        public JsonResult FindDomains(ProductSearchQuery searchQuery)
        {
            if (ModelState.IsValid)
            {
                var searchResults = productsProvider.GetProducts(searchQuery).Select(r => new ProductModel(r)).ToList();

                return JsonEnvelope.Success(searchResults);
            }

            return JsonEnvelope.Fail(ModelState);
        }
    }
}
