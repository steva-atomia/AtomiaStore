using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CurrencyProvider : PublicBillingApiClient, ICurrencyProvider
    {
        private readonly AccountData resellerData;
        private readonly IResourceProvider resourceProvider;

        public CurrencyProvider(IResellerDataProvider resellerDataProvider, IResourceProvider resourceProvider, PublicBillingApiProxy billingApi)
            : base(billingApi)
        {
            if (resellerDataProvider == null)
            {
                throw new ArgumentNullException("resellerDataProvider");
            }

            if (resourceProvider == null)
            {
                throw new ArgumentNullException("resourceProvider");
            }

            this.resellerData = resellerDataProvider.GetResellerAccountData();
            this.resourceProvider = resourceProvider;
        }


        /// <summary>
        /// Get currencies available for current reseller.
        /// </summary>
        public IList<Currency> GetAvailableCurrencies()
        {
            List<Currency> currencies = this.resellerData?.Currencies != null && this.resellerData.Currencies.Length > 0
                                  ? this.resellerData.Currencies.Select(code => Currency.CreateCurrency(this.resourceProvider, code)).ToList()
                                  : new List<Currency> { this.GetDefaultCurrency() };

            return currencies;
        }

        /// <summary>
        /// Get default currency for available reseller
        /// </summary>
        /// <exception cref="System.InvalidOperationException">If reseller or default currency code is not available</exception>
        public Currency GetDefaultCurrency()
        {
            if (resellerData == null || String.IsNullOrEmpty(resellerData.DefaultCurrencyCode))
            {
                throw new InvalidOperationException("Could not find currency code for reseller");
            }
            
            return Currency.CreateCurrency(this.resourceProvider, resellerData.DefaultCurrencyCode);
        }
    }
}
