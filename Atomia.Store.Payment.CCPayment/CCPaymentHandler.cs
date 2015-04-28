using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Collections.Generic;

namespace Atomia.Store.Payment.CCPayment
{
    public sealed class CCPaymentHandler : PaymentDataHandler
    {
        private readonly PaymentUrlProvider urlProvider;

        public CCPaymentHandler(PaymentUrlProvider urlProvider)
        {
            if (urlProvider == null)
            {
                throw new ArgumentNullException("urlProvider");
            }

            this.urlProvider = urlProvider;
        }

        public override string Id
        {
            get { return "CCPayment"; }
        }

        public override PaymentMethodEnum PaymentMethodType
        {
            get { return PaymentMethodEnum.PayByCard; }
        }

        public override PublicPaymentTransaction AmendPaymentTransaction(PublicPaymentTransaction transaction, PaymentData paymentData)
        {
            var form = paymentData.PaymentForm as CCPaymentForm;

            var attributes = new List<AttributeData>(transaction.Attributes);

            attributes.Add(new AttributeData { Name = "cc_number", Value = form.CardNumber });
            attributes.Add(new AttributeData { Name = "cc_name", Value = form.CardOwner });
            attributes.Add(new AttributeData { Name = "ccv", Value = form.CardSecurityCode }); // wrong abbreviation "ccv" is expected.
            attributes.Add(new AttributeData { Name = "expires_month", Value = form.ExpiresMonth.ToString() });
            attributes.Add(new AttributeData { Name = "expires_year", Value = form.ExpiresYear.ToString() });

            transaction.Attributes = attributes.ToArray();

            transaction.ReturnUrl = urlProvider.DefaultPaymentUrl;
            transaction = SetCancelUrl(transaction, urlProvider.CancelUrl);

            return transaction;
        }
    }
}
