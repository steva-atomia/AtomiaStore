using Atomia.Store.AspNetMvc.Providers;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public sealed class CategoryController : Controller
    {
        private readonly CategoryProvider productsProvider;

        public CategoryController()
        {
            this.productsProvider = DependencyResolver.Current.GetService<CategoryProvider>();
        }

        [HttpGet]
        public ActionResult ListProducts(string category, string viewName = "ListProducts")
        {
            var model = DependencyResolver.Current.GetService<CategoryViewModel>();

            model.Category = category;
            model.Products = productsProvider.GetProducts(new ProductSearchQuery
            {
                Terms = new List<SearchTerm>
                {
                    new SearchTerm 
                    {
                        Key = "category",
                        Value = category
                    }
                }
            }).Select(r => new ProductModel(r)).ToList();

            return View(viewName, model);
        }

        [ChildActionOnly]
        public PartialViewResult ListProductsPartial(string category, string viewName = "_ListProducts")
        {
            var model = DependencyResolver.Current.GetService<CategoryViewModel>();

            model.Category = category;
            model.Products = productsProvider.GetProducts(new ProductSearchQuery
            {
                Terms = new List<SearchTerm>
                {
                    new SearchTerm 
                    {
                        Key = "category",
                        Value = category
                    }
                }
            }).Select(r => new ProductModel(r)).ToList();

            return PartialView(viewName, model);
        }
    }
}
