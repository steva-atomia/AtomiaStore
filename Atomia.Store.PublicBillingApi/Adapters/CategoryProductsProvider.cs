using Atomia.Store.Core;
using Atomia.Web.Plugin.ProductsProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using CoreProduct = Atomia.Store.Core.Product;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    public sealed class CategoryProductsProvider : IProductListProvider
    {
        private readonly IResellerProvider resellerProvider;
        private readonly ILanguagePreferenceProvider languagePreferenceProvider;
        private readonly ICurrencyPreferenceProvider currencyPreferenceProvider;
        private readonly IProductsProvider productsProvider;

        public CategoryProductsProvider(IResellerProvider resellerProvider, ILanguagePreferenceProvider languagePreferenceProvider, ICurrencyPreferenceProvider currencyPreferenceProvider, IProductsProvider productsProvider)
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
                throw new ArgumentNullException("currencyPreferenceProvider");
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

        public string Name
        {
            get { return "Category"; }
        }

        public IEnumerable<CoreProduct> GetProducts(ICollection<SearchTerm> terms)
        {
            var category = terms.First().Value;
            var resellerId = resellerProvider.GetReseller().Id;
            var currencyCode = currencyPreferenceProvider.GetCurrentCurrency().Code;
            var language = languagePreferenceProvider.GetCurrentLanguage();
            var products = new List<CoreProduct>();

            var apiProducts = productsProvider.GetShopProductsByCategories(resellerId, null, new List<string>() { category });

            foreach(var apiProduct in apiProducts)
            {
                var product = ProductMapper.Map(apiProduct, language, currencyCode);
                
                products.Add(product);
            }

            return products.OrderBy(p => p.PricingVariants.Min(v => v.Price));
        }
    }
}
