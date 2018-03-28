﻿using Atomia.Store.AspNetMvc.Filters;
using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Controllers
{
    /// <summary>
    /// Checkout and related payment handling and terms of service.
    /// </summary>
    public sealed class CheckoutController : Controller
    {
        private readonly ICartProvider cartProvider = DependencyResolver.Current.GetService<ICartProvider>();
        private readonly IContactDataProvider contactDataProvider = DependencyResolver.Current.GetService<IContactDataProvider>();
        private readonly IOrderPlacementService orderPlacementService = DependencyResolver.Current.GetService<IOrderPlacementService>();
        private readonly ICartPricingService cartPricingService = DependencyResolver.Current.GetService<ICartPricingService>();
        private readonly ITermsOfServiceProvider tosProvider = DependencyResolver.Current.GetService<ITermsOfServiceProvider>();
        private readonly PaymentUrlProvider urlProvider = DependencyResolver.Current.GetService<PaymentUrlProvider>();
        private readonly IVatNumberValidator vatValidator = DependencyResolver.Current.GetService<IVatNumberValidator>();
        private readonly IVatDataProvider vatDataProvider = DependencyResolver.Current.GetService<IVatDataProvider>();

        /// <summary>
        /// Checkout page, part of order flow
        /// </summary>
        [OrderFlowFilter]
        [HttpGet]
        public ActionResult Index()
        {
            // Make sure cart is properly calculated.
            var cart = cartProvider.GetCart();
            cartPricingService.CalculatePricing(cart);

            // If VAT number was submitted, indicate a VAT check should be made
            ViewBag.CheckVAT = !String.IsNullOrEmpty(vatDataProvider.VatNumber);
            
            var model = DependencyResolver.Current.GetService<CheckoutViewModel>();

            return View(model);
        }

        /// <summary>
        /// Checkout form handler.
        /// </summary>
        [OrderFlowFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CheckoutViewModel model)
        {
            var cart = cartProvider.GetCart();
            var contactDataCollection = contactDataProvider.GetContactData();

            ViewBag.CheckVAT = !String.IsNullOrEmpty(vatDataProvider.VatNumber);

            if (cart.IsEmpty())
            {
                ModelState.AddModelError("cart", "Cart is empty");
            }
            else if (contactDataCollection == null)
            {
                ModelState.AddModelError("contactData", "Contact data is empty");
            }

            if (ModelState.IsValid)
            {
                // Recalculate cart one last time, to make sure e.g. setup fees are still there.
                cartPricingService.CalculatePricing(cart);

                var paymentData = new PaymentData
                {
                    Id = model.SelectedPaymentMethod.Id,
                    PaymentForm = model.SelectedPaymentMethod.Form,
                    SaveCcInfo = model.SelectedPaymentMethod.SupportsPaymentProfile && model.SaveCcInfo,
                    AutoPay = model.SelectedPaymentMethod.SupportsPaymentProfile && model.AutoPay
                };

                var orderContext = new OrderContext(cart, contactDataCollection, paymentData, new object[] { Request });
                var result = orderPlacementService.PlaceOrder(orderContext);
                
                if (result.RedirectUrl == urlProvider.SuccessUrl)
                {
                    contactDataProvider.ClearContactData();
                    cartProvider.ClearCart();
                }

                return Redirect(result.RedirectUrl);
            }

            return View(model);
        }

        /// <summary>
        /// Successful payment page.
        /// </summary>
        [HttpGet]
        public ActionResult Success()
        {
            contactDataProvider.ClearContactData();
            cartProvider.ClearCart();

            return View();
        }

        /// <summary>
        /// Failed payment page. 
        /// </summary>
        [HttpGet]
        public ActionResult Failure()
        {
            return View();
        }

        /// <summary>
        /// Page for redirect from Atomia payment HTTP handlers
        /// </summary>
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Payment(string amount, string transactionReference, int transactionReferenceType, string status)
        {
            var redirectUrl = urlProvider.FailureUrl;

            switch (status.ToUpper())
	        {
                case PaymentTransaction.Ok:
                case PaymentTransaction.InProgress:
                    contactDataProvider.ClearContactData();
                    cartProvider.ClearCart();
                    redirectUrl = urlProvider.SuccessUrl;
                    break;

                case PaymentTransaction.Failed:
		        default:
                    redirectUrl = urlProvider.FailureUrl;
                    break;
	        }

            return Redirect(redirectUrl);
        }

        /// <summary>
        /// Terms of service presentation page.
        /// </summary>
        [HttpGet]
        public ActionResult TermsOfService(string id)
        {
            var tos = tosProvider.GetTermsOfService(id);

            if (tos == null)
            {
                return HttpNotFound();
            }

            var model = new TermsOfServiceViewModel
            {
                Id = tos.Id,
                Name = tos.Name,
                Terms = tos.Terms,
                Link = tos.Link
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult TermsOfServiceObject(string id)
        {
            var tos = tosProvider.GetTermsOfService(id);

            if (tos == null)
            {
                return HttpNotFound();
            }

            var model = new TermsOfService
            {
                Id = tos.Id,
                Name = tos.Name,
                Terms = tos.Terms,
                Link = tos.Link
            };

            return Json(model);
        }

        /// <summary>
        /// Validate VAT number with backend service
        /// </summary>
        [HttpPost]
        public ActionResult ValidateVatNumber(string vatNumber)
        {
            if (!string.IsNullOrEmpty(vatNumber))
            {
                vatDataProvider.VatNumber = vatNumber;
            }

            var result = vatValidator.ValidateCustomerVatNumber();

            return JsonEnvelope.Success(result);
        }

        /// <summary>
        /// Validate VAT number with backend service
        /// </summary>
        [HttpPost]
        public ActionResult TermsOfService()
        {
            CheckoutViewModel checkoutModel = new DefaultCheckoutViewModel();

            return JsonEnvelope.Success(checkoutModel.TermsOfService);
        }
    }
}
