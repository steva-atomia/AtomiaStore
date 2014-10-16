using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atomia.OrderPage.Sdk.Core.Models;

namespace Atomia.OrderPage.Sdk.Core.Controllers
{
    public sealed class AddonsController : Controller
    {
        public AddonsController()
        {

        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(AddonsModel model)
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
