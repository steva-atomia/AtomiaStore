using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atomia.Store.Core;
using Atomia.Store.UI.Infrastructure;
using Atomia.Store.UI.ViewModels;

namespace Atomia.Store.UI.Controllers
{
    public sealed class ProductsController : Controller
    {
        private readonly IModelProvider modelProvider;

        public ProductsController(IModelProvider modelProvider)
        {
            this.modelProvider = modelProvider;
        }

        [HttpGet]
        public ActionResult ListCategory(string category)
        {
            var model = modelProvider.Create<PackageViewModel>();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(PackageViewModel model)
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult Partial()
        {
            return PartialView();
        }
    }
}
