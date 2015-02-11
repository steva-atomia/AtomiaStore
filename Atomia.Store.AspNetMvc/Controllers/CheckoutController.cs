using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public sealed class CheckoutController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = DependencyResolver.Current.GetService<CheckoutViewModel>();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CheckoutViewModel model)
        {
            if (model.SelectedPaymentMethod != null && model.SelectedPaymentMethod.PaymentForm != null)
            {
                TryUpdateModel(model.SelectedPaymentMethod.PaymentForm);
            }

            // ModelState.AddModelError("", 
            // ModelState.AddModelError("", 
            // Validate Cart (with TOS)
            // Validate AccountData

            if (ModelState.IsValid)
            {
                var orderPlacementService = DependencyResolver.Current.GetService<IOrderPlacementService>();

                var cartProvider = DependencyResolver.Current.GetService<ICartProvider>();
                var cart = cartProvider.GetCart();

                var contactDataProvider = DependencyResolver.Current.GetService<IContactDataProvider>();
                var contactDataCollection = contactDataProvider.GetContactData();

                var redirectUrl = orderPlacementService.PlaceOrder(cart, contactDataCollection, model.SelectedPaymentMethod);

                return Redirect(redirectUrl);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Success()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Failure()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult PaymentRedirect(string amount, string transactionReference, int transactionReferenceType, string status)
        {
            decimal decimalAmount;
            Decimal.TryParse(amount, out decimalAmount);

            if (status.ToUpper() == PaymentTransaction.Ok || status.ToUpper() == PaymentTransaction.InProgress)
            {
                return RedirectToAction("Success");
            }
            
            if (status.ToUpper() == PaymentTransaction.Failed)
            {
                return RedirectToAction("Failure");
            }
            
            return RedirectToAction("Failure");
        }
    }
}
