using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services.Protocols;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    public class OrderPlacementService : PublicBillingApiClient, IOrderPlacementService
    {
        private readonly PaymentUrlProvider urlProvider;
        private readonly IProductProvider productProvider;
        private readonly RenewalPeriodProvider renewalPeriodProvider;
        private readonly IEnumerable<OrderDataHandler> orderDataHandlers;
        private readonly IEnumerable<PaymentDataHandler> paymentDataHandlers;
        private readonly IEnumerable<TransactionDataHandler> transactionDataHandlers;
        private readonly ILogger logger;
        
        public OrderPlacementService(
            PaymentUrlProvider urlProvider,
            IProductProvider productProvider,
            RenewalPeriodProvider renewalPeriodProvider,
            IEnumerable<OrderDataHandler> orderDataHandlers,
            IEnumerable<PaymentDataHandler> paymentDataHandlers,
            IEnumerable<TransactionDataHandler> transactionDataHandlers,
            ILogger logger,
            PublicBillingApiProxy billingApi) 
            : base(billingApi)
        {
            if (urlProvider == null)
            {
                throw new ArgumentNullException("urlProvider");
            }

            if (productProvider == null)
            {
                throw new ArgumentNullException("productProvider");
            }

            if (renewalPeriodProvider == null)
            {
                throw new ArgumentNullException("renewalPeriodProvider");
            }

            if (orderDataHandlers == null)
            {
                throw new ArgumentNullException("orderDataHandlers");
            }

            if (paymentDataHandlers == null)
            {
                throw new ArgumentNullException("paymentDataHandlers");
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
            this.productProvider = productProvider;
            this.renewalPeriodProvider = renewalPeriodProvider;
            this.orderDataHandlers = orderDataHandlers;
            this.paymentDataHandlers = paymentDataHandlers;
            this.transactionDataHandlers = transactionDataHandlers;
            this.logger = logger;
        }

        public OrderResult PlaceOrder(OrderContext orderContext)
        {
            var publicOrderContext = new PublicOrderContext(orderContext);

            // Add some extra data that might be needed by order handlers.
            foreach (var cartItem in orderContext.Cart.CartItems)
            {
                var product = productProvider.GetProduct(cartItem.ArticleNumber);
                var renewalPeriodId = renewalPeriodProvider.GetRenewalPeriodId(cartItem);

                publicOrderContext.AddItemData(new ItemData(cartItem, product, renewalPeriodId));
            }

            var paymentHandler = paymentDataHandlers.FirstOrDefault(h => h.Id == orderContext.PaymentData.Id);
            if (paymentHandler == null)
            {
                throw new InvalidOperationException(String.Format("Payment data handler is not available for {0}.", orderContext.PaymentData.Id));
            }

            var createdOrder = CreateOrder(publicOrderContext, paymentHandler);

            var redirectUrl = urlProvider.SuccessUrl;

            if (paymentHandler.PaymentMethodType == PaymentMethodEnum.PayByCard && createdOrder.Total > Decimal.Zero)
            {
                redirectUrl = CreatePaymentTransaction(publicOrderContext, createdOrder, paymentHandler);
            }
            
            return new OrderResult
            {
                RedirectUrl = redirectUrl
            };
        }

        private PublicOrder CreateOrder(PublicOrderContext publicOrderContext, PaymentDataHandler paymentHandler)
        {
            var newOrder = new PublicOrder()
            {
                OrderItems = new PublicOrderItem[0],
                CustomData = new PublicOrderCustomData[0]
            };

            foreach (var handler in orderDataHandlers)
            {
                newOrder = handler.AmendOrder(newOrder, publicOrderContext);

                if (newOrder == null)
                {
                    throw new InvalidOperationException("OrderDataHandler must return a non-null order from AmendOrder.");
                }
            }

            newOrder.PaymentMethod = paymentHandler.PaymentMethodType;

            // Only run the selected payment handler.
            newOrder = paymentHandler.AmendOrder(newOrder, publicOrderContext.PaymentData);

            if (newOrder == null)
            {
                throw new InvalidOperationException("PaymentDataHandler must return a non-null order from AmendOrder.");
            }

            var createdOrder = BillingApi.CreateOrder(newOrder);

            if (createdOrder == null)
            {
                throw new InvalidOperationException("Order could not be created.");
            }

            return createdOrder;
        }


        private string CreatePaymentTransaction(PublicOrderContext publicOrderContext, PublicOrder order, PaymentDataHandler paymentHandler)
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
            catch(SoapException ex)
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
