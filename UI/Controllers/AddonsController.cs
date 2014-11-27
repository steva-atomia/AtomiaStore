using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atomia.Store.UI.ViewModels;

namespace Atomia.Store.UI.Controllers
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
        public ActionResult Index(AddonsViewModel model)
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
