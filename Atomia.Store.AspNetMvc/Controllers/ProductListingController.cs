using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public sealed class ProductListingController : Controller
    {
        private readonly IEnumerable<IProductListProvider> providers;

        public ProductListingController()
        {
            this.providers = DependencyResolver.Current.GetServices<IProductListProvider>();
        }

        [HttpGet]
        public ActionResult Index(string query, string listingType = "Category", string viewName = "Index")
        {
            var model = DependencyResolver.Current.GetService<ProductListingViewModel>();
            model.Query = query;

            return View(viewName, model);
        }

        [HttpGet]
        public JsonResult GetData(string query, string listingType = "Category")
        {
            var model = InitDataModel(query, listingType);

            return JsonEnvelope.Success(new
            {
                CategoryData = model
            });
        }

        private ProductListingDataModel InitDataModel(string query, string listingType)
        {
            var model = DependencyResolver.Current.GetService<ProductListingDataModel>();
            var provider = providers.FirstOrDefault(x => x.Name == listingType);

            if (provider == null)
            {
                throw new InvalidOperationException(String.Format("No IProductsProvider with Name \"{0}\" registered.", listingType));
            }

            var searchTerms = new List<SearchTerm> 
            {
                new SearchTerm(listingType, query)
            };

            model.Query = query;
            model.Products = provider.GetProducts(searchTerms).Select(r => new ProductModel(r)).ToList();

            return model;
        }
    }
}
