using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atomia.Store.Core;
using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.ViewModels;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public sealed class ProductsController : Controller
    {
        private readonly IModelProvider modelProvider;
        private readonly IProductsProvider productsProvider;

        public ProductsController(IModelProvider modelProvider, IProductsProvider productsProvider)
        {
            this.modelProvider = modelProvider;
            this.productsProvider = productsProvider;
        }

        [HttpGet]
        public ActionResult ListCategory(string category, string viewName="ListCategory")
        {
            var model = modelProvider.Create<ProductsViewModel>();
            model.Category = category;
            model.Products = productsProvider.GetProducts(category);

            return View(viewName, model);
        }

        [ChildActionOnly]
        public PartialViewResult ListCategoryPartial(string category)
        {
            var model = modelProvider.Create<ProductsViewModel>();
            model.Products = productsProvider.GetProducts(category);

            return PartialView(model);
        }
    }
}
