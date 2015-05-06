using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    /// <summary>
    /// Place an order to Atomia Billing via Atomia Billing Public Service.
    /// </summary>
    public sealed class OrderPlacementService : IOrderPlacementService
    {
        private readonly PaymentUrlProvider urlProvider;
        private readonly IProductProvider productProvider;
        private readonly RenewalPeriodProvider renewalPeriodProvider;
        private readonly IEnumerable<PaymentDataHandler> paymentDataHandlers;
        private readonly OrderCreator orderCreator;
        private readonly PaymentTransactionCreator paymentTransactionCreator;
        
        /// <summary>
        /// Create a new instance of the service.
        /// </summary>
        public OrderPlacementService(
            PaymentUrlProvider urlProvider,
            IProductProvider productProvider,
            RenewalPeriodProvider renewalPeriodProvider,
            IEnumerable<PaymentDataHandler> paymentDataHandlers,
            OrderCreator orderCreator,
            PaymentTransactionCreator paymentTransactionCreator)
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

            if (paymentDataHandlers == null)
            {
                throw new ArgumentNullException("paymentDataHandlers");
            }

            if (orderCreator == null)
            {
                throw new ArgumentNullException("orderCreator");
            }

            if (paymentTransactionCreator == null)
            {
                throw new ArgumentNullException("paymentTransactionCreator");
            }

            this.urlProvider = urlProvider;
            this.productProvider = productProvider;
            this.renewalPeriodProvider = renewalPeriodProvider;
            this.paymentDataHandlers = paymentDataHandlers;
            this.orderCreator = orderCreator;
            this.paymentTransactionCreator = paymentTransactionCreator;
        }

        /// <summary>
        /// Place the order with data collected in the provided <see cref="Atomia.Store.Core.OrderContext"/>.
        /// </summary>
        /// <param name="orderContext">Context with cart, contact and other relevant data.</param>
        /// <returns>The results of placing the order</returns>
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

            var createdOrder = orderCreator.CreateOrder(publicOrderContext, paymentHandler);

            var redirectUrl = urlProvider.SuccessUrl;

            if (paymentHandler.PaymentMethodType == PaymentMethodEnum.PayByCard && createdOrder.Total > Decimal.Zero)
            {
                redirectUrl = paymentTransactionCreator.CreatePaymentTransaction(publicOrderContext, createdOrder, paymentHandler);
            }
            
            return new OrderResult
            {
                RedirectUrl = redirectUrl
            };
        }
    }
}
