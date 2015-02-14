using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    public class CurrencyProvider : PublicBillingApiClient, ICurrencyProvider
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

        public IList<Currency> GetAvailableCurrencies()
        {
            // Only resellers default currency is exposed in public billing api.
            var currencies = new List<Currency> { GetDefaultCurrency() };

            return currencies;
        }

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
