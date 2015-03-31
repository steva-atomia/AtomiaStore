using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Collections.Generic;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CurrencyProvider : PublicBillingApiClient, ICurrencyProvider
    {
        private readonly AccountData resellerData;

        public CurrencyProvider(IResellerDataProvider resellerDataProvider, PublicBillingApiProxy billingApi)
            : base(billingApi)
        {
            if (resellerDataProvider == null)
            {
                throw new ArgumentNullException("resellerDataProvider");
            }

            this.resellerData = resellerDataProvider.GetResellerAccountData();
        }


        /// <summary>
        /// Get currencies available for current reseller.
        /// </summary>
        public IList<Currency> GetAvailableCurrencies()
        {
            // FIXME (2015-03-31): Only resellers default currency is exposed in public billing api, update when public billing api has been fixed.
            var currencies = new List<Currency> { GetDefaultCurrency() };

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
            
            return new Currency(resellerData.DefaultCurrencyCode);
        }
    }
}
