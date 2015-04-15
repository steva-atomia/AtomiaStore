using Atomia.Store.Core;
using Atomia.Web.Plugin.ProductsProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using CoreProduct = Atomia.Store.Core.Product;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    /// <summary>
    /// Provides product data from current reseller's shop in Atomia Billing.
    /// </summary>
    public sealed class ProductProvider : IProductProvider
    {
        private readonly ApiProductsProvider apiProductsProvider;
        private readonly ILanguagePreferenceProvider languagePreferenceProvider;
        private readonly ICurrencyPreferenceProvider currencyPreferenceProvider;

        /// <summary>
        /// Create new instance tied to current reseller and current users's language and currency preferences.
        /// </summary>
        public ProductProvider(ILanguagePreferenceProvider languagePreferenceProvider, ICurrencyPreferenceProvider currencyPreferenceProvider, ApiProductsProvider apiProductsProvider)
        {
            if (languagePreferenceProvider == null)
            {
                throw new ArgumentNullException("languagePreferenceProvider");
            }

            if (currencyPreferenceProvider == null)
            {
                throw new ArgumentNullException("currencyProvider");
            }

            if (apiProductsProvider == null)
            {
                throw new ArgumentNullException("apiProductsProvider");
            }

            this.languagePreferenceProvider = languagePreferenceProvider;
            this.currencyPreferenceProvider = currencyPreferenceProvider;
            this.apiProductsProvider = apiProductsProvider;
        }

        /// <summary>
        /// Get data for product with specified article number from current reseller's shop in Atomia Billing
        /// </summary>
        public CoreProduct GetProduct(string articleNumber)
        {
            var apiProduct = apiProductsProvider.GetProduct(articleNumber);
            var language = languagePreferenceProvider.GetCurrentLanguage();
            var currency = currencyPreferenceProvider.GetCurrentCurrency();

            var product = ProductMapper.Map(apiProduct, language, currency.Code);

            return product;
        }
    }
}
