using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System.Web.Mvc;
using Atomia.Store.AspNetMvc.Filters;

namespace Atomia.Store.AspNetMvc.Controllers
{
    /// <summary>
    /// Account data collection, part of order flow
    /// </summary>
    public sealed class AccountController : Controller
    {
        private readonly IContactDataProvider contactDataProvider = DependencyResolver.Current.GetService<IContactDataProvider>();

        /// <summary>
        /// Account form page.
        /// </summary>
        [HttpGet]
        [OrderFlowFilter]
        public ActionResult Index()
        {
            var model = DependencyResolver.Current.GetService<AccountViewModel>();

            return View(model);
        }

        /// <summary>
        /// Account form handler. Redirects to checkout.
        /// </summary>
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
