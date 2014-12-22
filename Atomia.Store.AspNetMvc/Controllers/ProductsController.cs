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
        private readonly Func<string, IProductsProvider> productsProviderFactory;

        public ProductsController(IModelProvider modelProvider, Func<string, IProductsProvider> productsProviderFactory)
        {
            this.modelProvider = modelProvider;
            this.productsProviderFactory = productsProviderFactory;
        }

        [HttpGet]
        public ActionResult ListProducts(ProductSearchQuery searchQuery, string productsProviderName, string viewName = "ListProducts")
        {
            var model = modelProvider.Create<ListProductsViewModel>();
            model.Products = GetSearchResults(searchQuery, productsProviderName);

            return View(viewName, model);
        }

        [ChildActionOnly]
        public PartialViewResult ListProductsPartial(ProductSearchQuery searchQuery, string productsProviderName, string viewName = "_ListProducts")
        {
            var model = modelProvider.Create<ListProductsViewModel>();
            model.Products = GetSearchResults(searchQuery, productsProviderName);

            return PartialView(viewName, model);
        }

        [HttpGet]
        public ActionResult SearchForProducts(string productsProviderName, string viewName = "SearchProducts")
        {
            var model = modelProvider.Create<SearchProductsViewModel>();

            return View(viewName, model);
        }

        [HttpGet]
        public JsonResult FindProducts(ProductSearchQuery searchQuery, string productsProviderName)
        {
            if (ModelState.IsValid)
            {
                var searchResults = GetSearchResults(searchQuery, productsProviderName);

                return JsonEnvelope.Success(searchResults);
            }

            return JsonEnvelope.Fail(ModelState);
        }

        private ICollection<ProductModel> GetSearchResults(ProductSearchQuery searchQuery, string productsProviderName)
        {
            var productsProvider = productsProviderFactory(productsProviderName);
            return productsProvider.GetProducts(searchQuery).Select(r => new ProductModel(r)).ToList();
        }
    }
}
