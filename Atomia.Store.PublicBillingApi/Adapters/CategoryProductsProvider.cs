using Atomia.Store.Core;
using Atomia.Web.Plugin.ProductsProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using CoreProduct = Atomia.Store.Core.Product;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    /// <summary>
    /// <see cref="Atomia.Store.Core.IProductListProvider"/> to get products ín specific category from current reseller's shop in Atomia Billing.
    /// </summary>
    public sealed class CategoryProductsProvider : IProductListProvider
    {
        private readonly ILanguagePreferenceProvider languagePreferenceProvider;
        private readonly ICurrencyPreferenceProvider currencyPreferenceProvider;
        private readonly ApiProductsProvider apiProductsProvider;

        /// <summary>
        /// Construct a new instance
        /// </summary>
        public CategoryProductsProvider(ILanguagePreferenceProvider languagePreferenceProvider, ICurrencyPreferenceProvider currencyPreferenceProvider, ApiProductsProvider apiProductsProvider)
        {
            

            if (languagePreferenceProvider == null)
            {
                throw new ArgumentNullException("languagePreferenceProvider");
            }

            if (currencyPreferenceProvider == null)
            {
                throw new ArgumentNullException("currencyPreferenceProvider");
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
        /// The unique name of the <see cref="Atomia.Store.Core.IProductListProvider"/>
        /// </summary>
        public string Name
        {
            get { return "Category"; }
        }

        /// <summary>
        /// Get products by category. The value of the first <see cref="Atomia.Store.Core.SearchTerm"/> will be used as the category name to list products from.
        /// </summary>
        /// <param name="terms">Search terms, the first of which should have the value of category to find products from</param>
        /// <returns>Any found products for the category, ordered by price.</returns>
        public IEnumerable<CoreProduct> GetProducts(ICollection<SearchTerm> terms)
        {
            var category = terms.First().Value;
            var currencyCode = currencyPreferenceProvider.GetCurrentCurrency().Code;
            var language = languagePreferenceProvider.GetCurrentLanguage();
            var products = new List<CoreProduct>();

            var apiProducts = apiProductsProvider.GetProductsByCategories(new List<string>() { category });

            foreach(var apiProduct in apiProducts)
            {
                var product = ProductMapper.Map(apiProduct, language, currencyCode);
                
                products.Add(product);
            }

            return products.OrderBy(p => p.PricingVariants.Min(v => v.Price));
        }
    }
}
