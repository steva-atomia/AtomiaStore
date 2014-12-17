using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System.Linq;
using System.Web.Mvc;

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
            var model = CreateModel(category);

            return View(viewName, model);
        }

        [ChildActionOnly]
        public PartialViewResult ListCategoryPartial(string category, string viewName = "_ListCategory")
        {
            var model = CreateModel(category);

            return PartialView(viewName, model);
        }

        private ListCategoryViewModel CreateModel(string category)
        {
            var model = modelProvider.Create<ListCategoryViewModel>();

            model.Category = category;
            model.Products = productsProvider.GetProducts(category).Select(p => new ProductModel(p)).ToList();

            return model;
        }
    }
}
