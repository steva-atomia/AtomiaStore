using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Atomia.Store.PublicBillingApi
{
    public abstract class OrderCreator : PublicBillingApiClient
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

        protected PublicOrder PrepareOrder(PublicOrderContext publicOrderContext, PaymentDataHandler paymentHandler)
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

            return newOrder;
        }

        public abstract PublicOrder CreateOrder(PublicOrderContext publicOrderContext, PaymentDataHandler paymentHandler);
    }


    public class SimpleOrderCreator : OrderCreator
    {
        public SimpleOrderCreator(IEnumerable<OrderDataHandler> orderDataHandlers, 
            PublicBillingApiProxy billingApi) : base(orderDataHandlers, billingApi) 
        { }

        /// <summary>
        /// Create PublicOrder and call CreateOrder in Atomia Billing Public Service.
        /// </summary>
        /// <param name="publicOrderContext">Order data</param>
        /// <param name="paymentHandler">Handler for customer's selected payment method</param>
        /// <returns>The order object returned from Atomia Billing Public Service</returns>
        public override PublicOrder CreateOrder(PublicOrderContext publicOrderContext, PaymentDataHandler paymentHandler)
        {
            var newOrder = PrepareOrder(publicOrderContext, paymentHandler);

            var createdOrder = BillingApi.CreateOrder(newOrder);

            if (createdOrder == null)
            {
                throw new InvalidOperationException("Order could not be created.");
            }

            return createdOrder;
        }
    }


    public class TokenLoginOrderCreator : OrderCreator
    {
        private readonly PaymentUrlProvider urlProvider;

        public TokenLoginOrderCreator(PaymentUrlProvider urlProvider, IEnumerable<OrderDataHandler> orderDataHandlers, PublicBillingApiProxy billingApi) 
            : base(orderDataHandlers, billingApi) 
        { 
            if (urlProvider == null)
            {
                throw new ArgumentException("urlProvider");
            }

            this.urlProvider = urlProvider;
        }

        public override PublicOrder CreateOrder(PublicOrderContext publicOrderContext, PaymentDataHandler paymentHandler)
        {
            var newOrder = this.PrepareOrder(publicOrderContext, paymentHandler);
            this.AddCustomAttributes(newOrder);

            string token = string.Empty;
            var resellerRootDomain = this.GetResellerRootDomain();

            var createdOrder = BillingApi.CreateOrderWithLoginToken(newOrder, resellerRootDomain, out token);
            if (createdOrder == null)
            {
                throw new InvalidOperationException("Order could not be created.");
            }

            urlProvider.SuccessUrl = GetTokenLoginUrl(createdOrder.Email, token);

            return createdOrder;
        }

        private void AddCustomAttributes(PublicOrder newOrder)
        {
            var customAttributes = new List<PublicOrderCustomData>(newOrder.CustomData);

            customAttributes.Add(new PublicOrderCustomData { Name = "ImmediateProvisioning", Value = "true" });
            customAttributes.Add(new PublicOrderCustomData { Name = "ShowHcpLandingPage", Value = "true" });

            newOrder.CustomData = customAttributes.ToArray();
        }

        private string GetResellerRootDomain()
        {
            var uri = HttpContext.Current.Request.Url.AbsoluteUri;
            var resellerRootDomain = String.Join(".", new Uri(uri).Host.Split('.').Skip(1).ToArray());

            return resellerRootDomain;
        }

        private string GetTokenLoginUrl(string username, string token)
        {
            string url = string.Empty;
            var tokenLoginUrl = ConfigurationManager.AppSettings["TokenLoginUrl"];

            if (string.IsNullOrEmpty(tokenLoginUrl))
            {
                throw new Exception("Missing appSetting with key TokenLoginUrl");
            }

            return string.Format("{0}?username={1}&token={2}", tokenLoginUrl, HttpUtility.UrlEncode(username), token);
        }
    }
}
