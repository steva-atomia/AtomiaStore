using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Atomia.Store.Payment.PayPal
{
    public class PayPalHandler : PaymentDataHandler
    {
        private readonly PaymentUrlProvider urlProvider;

        public PayPalHandler(PaymentUrlProvider urlProvider)
        {
            if (urlProvider == null)
            {
                throw new ArgumentNullException("urlProvider");
            }

            this.urlProvider = urlProvider;
        }

        public override string Id
        {
            get { return "PayPal"; }
        }

        public override PaymentMethodEnum PaymentMethodType
        {
            get { return PaymentMethodEnum.PayByCard; }
        }

        public override PublicPaymentTransaction AmendPaymentTransaction(PublicPaymentTransaction transaction, PaymentData paymentData)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext, RouteTable.Routes);
            var path = urlHelper.Action("Confirm", "PayPal");

            transaction.ReturnUrl = urlProvider.QualifiedUrl(path);
            transaction = SetCancelUrl(transaction, urlProvider.CancelUrl);

            return transaction;
        }
    }
}
