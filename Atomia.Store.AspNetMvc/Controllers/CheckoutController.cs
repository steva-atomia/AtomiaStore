using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;
using Atomia.Store.AspNetMvc.Ports;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public sealed class CheckoutController : Controller
    {
        private readonly IEnumerable<PaymentMethodForm> paymentMethodForms = DependencyResolver.Current.GetServices<PaymentMethodForm>();
        private readonly ICartProvider cartProvider = DependencyResolver.Current.GetService<ICartProvider>();
        private readonly IContactDataProvider contactDataProvider = DependencyResolver.Current.GetService<IContactDataProvider>();
        private readonly IOrderPlacementService orderPlacementService = DependencyResolver.Current.GetService<IOrderPlacementService>();
        private readonly ICartPricingService cartPricingService = DependencyResolver.Current.GetService<ICartPricingService>();
        
        [HttpGet]
        public ActionResult Index()
        {
            // Make sure cart is properly calculated.
            var cart = cartProvider.GetCart();
            cartPricingService.CalculatePricing(cart);

            var model = DependencyResolver.Current.GetService<CheckoutViewModel>();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CheckoutViewModel model)
        {
            // TODO: Add cart and contactdata validation.
            if (ModelState.IsValid)
            {
                var cart = cartProvider.GetCart();
                var contactDataCollection = contactDataProvider.GetContactData();
                
                // Recalculate cart one last time, to make sure e.g. setup fees are still there.
                cartPricingService.CalculatePricing(cart);

                var orderContext = new OrderContext(cart, contactDataCollection, model.SelectedPaymentMethod, new object[] { Request });
                var result = orderPlacementService.PlaceOrder(orderContext);

                return Redirect(result.RedirectUrl);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Success()
        {
            contactDataProvider.ClearContactData();
            cartProvider.ClearCart();

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
