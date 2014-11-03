using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atomia.OrderPage.Core.Models;
using Atomia.OrderPage.Core.Infrastructure;

namespace Atomia.OrderPage.Core.Controllers
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
            var model = modelProvider.Create<PackageModel>();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(PackageModel model)
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
