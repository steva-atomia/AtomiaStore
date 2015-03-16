using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System.Web.Mvc;
using Atomia.Store.AspNetMvc.Filters;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public sealed class AccountController : Controller
    {
        private readonly IContactDataProvider contactDataProvider = DependencyResolver.Current.GetService<IContactDataProvider>();

        [OrderFlowFilter]
        [HttpGet]
        public ActionResult Index()
        {
            var model = DependencyResolver.Current.GetService<AccountViewModel>();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                contactDataProvider.SaveContactData(model);
                
                return RedirectToAction("Index", "Checkout");
            }

            return View(model);
        }
    }
}
