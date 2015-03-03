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
        private readonly IEnumerable<IProductListProvider> productListProviders = DependencyResolver.Current.GetServices<IProductListProvider>();
        private readonly IProductProvider productProvider = DependencyResolver.Current.GetService<IProductProvider>();

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

        public JsonResult GetItem(string articleNumber)
        {
            if (ModelState.IsValid)
            {
                Product product = null;

                try
                {
                    product = productProvider.GetProduct(articleNumber);
                }
                catch(ArgumentException)
                { 
                }

                if (product != null)
                {
                    return JsonEnvelope.Success(new
                    {
                        Item = new ProductModel(product)
                    });
                }
                else
                {
                    return JsonEnvelope.Fail(new
                    {
                        Message = String.Format("No product with article number {0}", articleNumber)
                    });
                }
            }

            return JsonEnvelope.Fail(ModelState);
        }

        private ProductListingDataModel InitDataModel(string query, string listingType)
        {
            var model = DependencyResolver.Current.GetService<ProductListingDataModel>();
            var provider = productListProviders.FirstOrDefault(x => x.Name == listingType);

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
