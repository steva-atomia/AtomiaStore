using Atomia.Store.Core;
using Atomia.Web.Plugin.ProductsProvider;
using System;
using System.Collections.Generic;
using System.Linq;

using Atomia.Web.Plugin.ShopNameProvider;

namespace Atomia.Store.PublicBillingApi
{
    public class ApiProductsProvider
    {
        private readonly IProductsProvider productsProvider;
        private readonly string shopName;
        private readonly Guid resellerId;

        public ApiProductsProvider(IProductsProvider productsProvider, IShopNameProvider shopNameProvider, IResellerProvider resellerProvider)
        {
            if (productsProvider == null)
            {
                throw new ArgumentNullException("productsProvider");
            }

            if (shopNameProvider == null)
            {
                throw new ArgumentNullException("shopNameProvider");
            }

            if (resellerProvider == null)
            {
                throw new ArgumentNullException("resellerProvider");
            }

            this.productsProvider = productsProvider;
            this.resellerId = resellerProvider.GetReseller().Id;
            this.shopName = shopNameProvider.GetShopName();
        }

        public IList<Web.Plugin.ProductsProvider.Product> GetProductsByCategories(IList<string> categories)
        {
            var apiProducts = productsProvider.GetShopProductsByCategories(this.resellerId, this.shopName, categories);

            return apiProducts;
        }

        public Web.Plugin.ProductsProvider.Product GetProduct(string articleNumber)
        {
            var apiProducts = productsProvider.GetShopProductsByArticleNumbers(this.resellerId, this.shopName, new List<string> { articleNumber });
           
            if (apiProducts == null || apiProducts.Count == 0)
            {
                throw new ArgumentException(String.Format("Could not find product with article number {0} for current reseller with shop {1}.", articleNumber, this.shopName));
            }

            return apiProducts.First();
        }
    }
}
