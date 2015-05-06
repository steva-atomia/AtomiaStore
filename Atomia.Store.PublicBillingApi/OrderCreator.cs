using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services.Protocols;

namespace Atomia.Store.PublicBillingApi
{
    public class OrderCreator : PublicBillingApiClient
    {
        private readonly IEnumerable<OrderDataHandler> orderDataHandlers;

        public OrderCreator(IEnumerable<OrderDataHandler> orderDataHandlers, PublicBillingApiProxy billingApi)
            : base(billingApi)
        {
            if (orderDataHandlers == null)
            {
                throw new ArgumentNullException("orderDataHandlers");
            }

            this.orderDataHandlers = orderDataHandlers;
        }

        /// <summary>
        /// Create PublicOrder and call CreateOrder in Atomia Billing Public Service.
        /// </summary>
        /// <param name="publicOrderContext">Order data</param>
        /// <param name="paymentHandler">Handler for customer's selected payment method</param>
        /// <returns>The order object returned from Atomia Billing Public Service</returns>
        public PublicOrder CreateOrder(PublicOrderContext publicOrderContext, PaymentDataHandler paymentHandler)
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
    }
}
