using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    /// <summary>
    /// Provides default and available payment methods from Atomia Billing for current reseller.
    /// </summary>
    public sealed class PaymentMethodsProvider : PublicBillingApiClient, IPaymentMethodsProvider
    {
        private readonly AccountData resellerData;

        /// <summary>
        /// Create new instance connected to current reseller
        /// </summary>
        public PaymentMethodsProvider(IResellerDataProvider resellerDataProvider, PublicBillingApiProxy billingApi)
            : base(billingApi)
        {
            if (resellerDataProvider == null)
            {
                throw new ArgumentNullException("resellerDataProvider");
            }

            resellerData = resellerDataProvider.GetResellerAccountData();
        }

        /// <summary>
        /// Get current reseller's available payment methods.
        /// </summary>
        public IEnumerable<Core.PaymentMethod> GetPaymentMethods()
        {
            var paymentMethods = resellerData.PaymentMethods.Select(p => new Core.PaymentMethod
            {
                Id = p.GuiPluginName
            });

            return paymentMethods;
        }

        /// <summary>
        /// Get current reseller's default payment method.
        /// </summary>
        public Core.PaymentMethod GetDefaultPaymentMethod()
        {
            var defaultPaymentMethod = new Core.PaymentMethod
            {
                Id = resellerData.DefaultPaymentMethod.GuiPluginName
            };

            return defaultPaymentMethod;
        }
    }
}
