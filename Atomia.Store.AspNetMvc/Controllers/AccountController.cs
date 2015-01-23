using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public sealed class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = DependencyResolver.Current.GetService<AccountViewModel>();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Save account state in session
                // TODO: Redirect to checkout
            }

            return View(model);
        }
    }
}
