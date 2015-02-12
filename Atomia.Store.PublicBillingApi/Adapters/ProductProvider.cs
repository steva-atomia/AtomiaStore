using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;
using Atomia.Web.Plugin.ProductsProvider;
using CoreProduct = Atomia.Store.Core.Product;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    public class ProductProvider : IProductProvider
    {
        private readonly IProductsProvider productsProvider;
        private readonly IResellerProvider resellerProvider;
        private readonly ILanguagePreferenceProvider languagePreferenceProvider;
        private readonly ICurrencyProvider currencyProvider;

        public ProductProvider(IResellerProvider resellerProvider, ILanguagePreferenceProvider languagePreferenceProvider, ICurrencyProvider currencyProvider, IProductsProvider productsProvider)
        {
            if (resellerProvider == null)
            {
                throw new ArgumentNullException("resellerProvider");
            }

            if (languagePreferenceProvider == null)
            {
                throw new ArgumentNullException("languagePreferenceProvider");
            }

            if (currencyProvider == null)
            {
                throw new ArgumentNullException("currencyProvider");
            }

            if (productsProvider == null)
            {
                throw new ArgumentNullException("productsProvider");
            }

            this.resellerProvider = resellerProvider;
            this.languagePreferenceProvider = languagePreferenceProvider;
            this.currencyProvider = currencyProvider;
            this.productsProvider = productsProvider;
        }

        public CoreProduct GetProduct(string articleNumber)
        {
            var resellerId = resellerProvider.GetReseller().Id;

            var apiProduct = productsProvider.GetShopProductsByArticleNumbers(resellerId, "", new List<string> { articleNumber }).FirstOrDefault();
            
            if (apiProduct == null)
            {
                throw new ArgumentException(String.Format("Could not find product with article number {0} for current reseller.", articleNumber));
            }

            var language = languagePreferenceProvider.GetPreferredLanguage();
            var currencyCode = currencyProvider.GetCurrencyCode();

            var product = ProductMapper.Map(apiProduct, language, currencyCode);

            return product;
        }
    }
}
