using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Atomia.OrderPage.UI.Controllers
{
    public sealed class CartController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Partial()
        {
            return PartialView();
        }

        public JsonResult AddItem()
        {
            return new JsonResult();
        }

        public JsonResult RemoveItem()
        {
            return new JsonResult();
        }
    }
}
