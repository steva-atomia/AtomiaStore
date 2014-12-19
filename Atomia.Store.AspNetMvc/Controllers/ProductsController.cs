using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public sealed class ProductsController : Controller
    {
        private readonly IModelProvider modelProvider;
        private readonly Func<string, IProductsProvider> productProviderFactory;

        public ProductsController(IModelProvider modelProvider, Func<string, IProductsProvider> productProviderFactory)
        {
            this.modelProvider = modelProvider;
            this.productProviderFactory = productProviderFactory;
        }

        [HttpGet]
        public ActionResult ListProducts(ProductSearchQuery searchQuery, string productProvider, string viewName = "ListProducts")
        {
            var model = CreateModel(searchQuery, productProvider);

            return View(viewName, model);
        }

        [ChildActionOnly]
        public PartialViewResult ListProductsPartial(ProductSearchQuery searchQuery, string productProvider, string viewName = "_ListProducts")
        {
            var model = CreateModel(searchQuery, productProvider);

            return PartialView(viewName, model);
        }

        [HttpGet]
        public JsonResult FindProducts(ProductSearchQuery searchQuery, string productProvider)
        {
            if (ModelState.IsValid)
            {
                var searchResults = GetSearchResults(searchQuery, productProvider);

                return JsonEnvelope.Success(searchResults);
            }

            return JsonEnvelope.Fail(ModelState);
        }

        private ListProductsViewModel CreateModel(ProductSearchQuery searchQuery, string productProvider)
        {
            var model = modelProvider.Create<ListProductsViewModel>();

            model.Category = searchQuery.Terms.FirstOrDefault(t => t.Name == "Category").Value;
            model.Products = GetSearchResults(searchQuery, productProvider);

            return model;
        }

        private ICollection<ProductModel> GetSearchResults(ProductSearchQuery searchQuery, string productProvider)
        {
            var provider = productProviderFactory(productProvider);
            return provider.GetProducts(searchQuery).Select(r => new ProductModel(r)).ToList();
        }
    }
}
