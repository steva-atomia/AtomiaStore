using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;
using Atomia.Web.Plugin.ProductsProvider;
using CoreProduct = Atomia.Store.Core.Product;
using ApiProduct = Atomia.Web.Plugin.ProductsProvider.Product;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    public class CategoryProductsProvider : IProductListProvider
    {
        private readonly IResellerProvider resellerProvider;
        private readonly ILanguagePreferenceProvider languagePreferenceProvider;
        private readonly ICurrencyProvider currencyProvider;
        private readonly IProductsProvider productsProvider;

        public CategoryProductsProvider(IResellerProvider resellerProvider, ILanguagePreferenceProvider languagePreferenceProvider, ICurrencyProvider currencyProvider, IProductsProvider productsProvider)
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

        public string Name
        {
            get { return "Category"; }
        }

        public IEnumerable<CoreProduct> GetProducts(ICollection<SearchTerm> terms)
        {
            var category = terms.First().Value;
            var resellerId = resellerProvider.GetReseller().Id;
            var currencyCode = currencyProvider.GetCurrencyCode();
            var language = languagePreferenceProvider.GetPreferredLanguage();
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
