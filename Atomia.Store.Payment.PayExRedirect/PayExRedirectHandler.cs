using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;

namespace Atomia.Store.Payment.PayExRedirect
{
    public class PayExRedirectHandler : PaymentDataHandler
    {
        private readonly PaymentUrlProvider urlProvider;

        public PayExRedirectHandler(PaymentUrlProvider urlProvider)
        {
            if (urlProvider == null)
            {
                throw new ArgumentNullException("urlProvider");
            }

            this.urlProvider = urlProvider;
        }

        public override string Id
        {
            get { return "PayExRedirect"; }
        }

        public override PaymentMethodEnum PaymentMethodType
        {
            get { return PaymentMethodEnum.PayByCard; }
        }

        public override PublicPaymentTransaction AmendPaymentTransaction(PublicPaymentTransaction transaction, PaymentData paymentData)
        {
            transaction.ReturnUrl = urlProvider.DefaultPaymentUrl;
            transaction = SetCancelUrl(transaction, urlProvider.CancelUrl);
            
            return transaction;
        }
    }
}
