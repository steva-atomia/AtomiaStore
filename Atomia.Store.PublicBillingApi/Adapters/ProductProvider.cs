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
        private readonly IProductsProvider productsProvider;
        private readonly IResellerProvider resellerProvider;
        private readonly ILanguagePreferenceProvider languagePreferenceProvider;
        private readonly ICurrencyPreferenceProvider currencyPreferenceProvider;

        /// <summary>
        /// Create new instance tied to current reseller and current users's language and currency preferences.
        /// </summary>
        public ProductProvider(IResellerProvider resellerProvider, ILanguagePreferenceProvider languagePreferenceProvider, ICurrencyPreferenceProvider currencyPreferenceProvider, IProductsProvider productsProvider)
        {
            if (resellerProvider == null)
            {
                throw new ArgumentNullException("resellerProvider");
            }

            if (languagePreferenceProvider == null)
            {
                throw new ArgumentNullException("languagePreferenceProvider");
            }

            if (currencyPreferenceProvider == null)
            {
                throw new ArgumentNullException("currencyProvider");
            }

            if (productsProvider == null)
            {
                throw new ArgumentNullException("productsProvider");
            }

            this.resellerProvider = resellerProvider;
            this.languagePreferenceProvider = languagePreferenceProvider;
            this.currencyPreferenceProvider = currencyPreferenceProvider;
            this.productsProvider = productsProvider;
        }

        /// <summary>
        /// Get data for product with specified article number from current reseller's shop in Atomia Billing
        /// </summary>
        public CoreProduct GetProduct(string articleNumber)
        {
            var resellerId = resellerProvider.GetReseller().Id;

            var apiProduct = productsProvider.GetShopProductsByArticleNumbers(resellerId, "", new List<string> { articleNumber }).FirstOrDefault();
            
            if (apiProduct == null)
            {
                throw new ArgumentException(String.Format("Could not find product with article number {0} for current reseller.", articleNumber));
            }
            
            var language = languagePreferenceProvider.GetCurrentLanguage();
            var currency = currencyPreferenceProvider.GetCurrentCurrency();

            var product = ProductMapper.Map(apiProduct, language, currency.Code);

            return product;
        }
    }
}
