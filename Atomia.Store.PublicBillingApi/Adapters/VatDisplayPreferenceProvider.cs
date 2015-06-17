using Atomia.Store.Core;
using System;
using System.Configuration;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    // Note: Added VatDisplayPreferenceProvider under this namespace in case we decide to move the setting to be backed by shops in Billing. 

    public class VatDisplayPreferenceProvider : IVatDisplayPreferenceProvider
    {
        public bool ShowPricesIncludingVat()
        {
            bool includeVat;
            var includeVatSetting = ConfigurationManager.AppSettings["PricesIncludeVat"];

            if (!Boolean.TryParse(includeVatSetting, out includeVat))
            {
                throw new ConfigurationErrorsException("Could not parse boolean from 'PricesIncludeVat' setting or it is missing.");
            }

            return includeVat;
        }
    }
}
