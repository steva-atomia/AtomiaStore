using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Collections.Generic;
using System.Web.Services.Protocols;

namespace Atomia.Store.PublicBillingApi
{
    public class PaymentTransactionCreator : PublicBillingApiClient
    {
        private readonly PaymentUrlProvider urlProvider;
        private readonly IEnumerable<TransactionDataHandler> transactionDataHandlers;
        private readonly ILogger logger;

        public PaymentTransactionCreator(
            PaymentUrlProvider urlProvider,
            IEnumerable<TransactionDataHandler> transactionDataHandlers,
            ILogger logger,
            PublicBillingApiProxy billingApi)
            : base(billingApi)
        {
            if (urlProvider == null)
            {
                throw new ArgumentNullException("urlProvider");
            }

            if (transactionDataHandlers == null)
            {
                throw new ArgumentNullException("transactionDataHandlers");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this.urlProvider = urlProvider;
            this.transactionDataHandlers = transactionDataHandlers;
            this.logger = logger;
        }

        /// <summary>
        /// Create PublicPaymentTransaction and call MakePayment in Atomia Billing Public Service.
        /// </summary>
        /// <param name="publicOrderContext">Order data</param>
        /// <param name="order">The order object returned from CreateOrder call in Atomia Billing Public Service</param>
        /// <param name="paymentHandler">Handler for customer's selected payment method</param>
        /// <returns>URL to redirect to for finishing or seeing result of payment transaction.</returns>
        public string CreatePaymentTransaction(PublicOrderContext publicOrderContext, PublicOrder order, PaymentDataHandler paymentHandler)
        {
            var paymentTransaction = new PublicPaymentTransaction
            {
                GuiPluginName = paymentHandler.Id,
                Attributes = new AttributeData[0],
                CurrencyCode = order.Currency,
                TransactionReference = order.Number,
                Amount = order.Total
            };

            paymentTransaction = paymentHandler.AmendPaymentTransaction(paymentTransaction, publicOrderContext.PaymentData);

            if (paymentTransaction == null)
            {
                throw new InvalidOperationException("PaymentDataHandler must return a non-null payment transaction from AmendTransaction.");
            }

            foreach (var handler in transactionDataHandlers)
            {
                paymentTransaction = handler.AmendPaymentTransaction(paymentTransaction, publicOrderContext);

                if (paymentTransaction == null)
                {
                    throw new InvalidOperationException("ExtraTransactionDataHandlers must return a non-null payment transaction from AmendTransaction.");
                }
            }

            PublicPaymentTransaction createdTransaction;

            try
            {
                createdTransaction = BillingApi.MakePayment(paymentTransaction);
            }
            catch (SoapException ex)
            {
                logger.LogException(ex, "MakePayment failed.");

                return urlProvider.FailureUrl;
            }

            if (createdTransaction.Status.ToUpper() == "IN_PROGRESS" && !string.IsNullOrEmpty(createdTransaction.RedirectUrl))
            {
                return createdTransaction.RedirectUrl;
            }
            else if (createdTransaction.Status.ToUpper() == "OK")
            {
                return urlProvider.SuccessUrl;
            }
            else if (createdTransaction.Status.ToUpper() == "FRAUD_DETECTED" || createdTransaction.Status.ToUpper() == "FAILED")
            {
                return urlProvider.FailureUrl;
            }
            else
            {
                return createdTransaction.ReturnUrl;
            }
        }
    }
}
