using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;

namespace Atomia.Store.PublicOrderHandlers
{
    /// <summary>
    /// Handler to set order currency from customer's currency preference
    /// </summary>
    public class CurrencyHandler : OrderDataHandler
    {
        private readonly ICurrencyPreferenceProvider currencyPreferenceProvider;

        /// <summary>
        /// Create new instance with access to customer's currency preference.
        /// </summary>
        public CurrencyHandler(ICurrencyPreferenceProvider currencyPreferenceProvider)
        {
            if (currencyPreferenceProvider == null)
            {
                throw new ArgumentNullException("currencyPreferenceProvider");
            }

            this.currencyPreferenceProvider = currencyPreferenceProvider;
        }

        /// <summary>
        /// Set order currency from customer's currency preference
        /// </summary>
        public override PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext)
        {
            var currency = currencyPreferenceProvider.GetCurrentCurrency();

            order.Currency = currency.Code;

            return order;
        }
    }
}
