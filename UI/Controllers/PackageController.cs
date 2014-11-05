using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atomia.OrderPage.Core.Models;
using Atomia.OrderPage.UI.Infrastructure;
using Atomia.OrderPage.UI.ViewModels;

namespace Atomia.OrderPage.UI.Controllers
{
    public sealed class PackageController : Controller
    {
        private readonly IModelProvider modelProvider;

        public PackageController(IModelProvider modelProvider)
        {
            this.modelProvider = modelProvider;
        }

        [HttpGet]
        public ActionResult Index()
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
