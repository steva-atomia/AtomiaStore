using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.AspNetMvc.Providers;
using Atomia.Store.Core;
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
        public ActionResult Index(string viewName = "ListProducts")
        {
            return View(viewName);
        }

        [HttpGet]
        public ActionResult ListCategory(string category, string viewName = "ListProducts")
        {
            var model = InitViewModel(category);

            return View(viewName, model);
        }

        [ChildActionOnly]
        public JsonResult ListCategoryData(string category)
        {
            var model = InitViewModel(category);

            return JsonEnvelope.Success(new
            {
                CategoryData = model
            });
        }

        private CategoryViewModel InitViewModel(string category)
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

            return model;
        }
    }
}
