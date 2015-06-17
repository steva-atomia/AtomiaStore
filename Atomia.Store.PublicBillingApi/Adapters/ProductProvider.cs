using Atomia.Store.Core;
using System;
using CoreProduct = Atomia.Store.Core.Product;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    /// <summary>
    /// Provides product data from current reseller's shop in Atomia Billing.
    /// </summary>
    public sealed class ProductProvider : IProductProvider
    {
        private readonly ApiProductsProvider apiProductsProvider;
        private readonly ProductMapper productMapper;

        public ProductProvider(ApiProductsProvider apiProductsProvider, ProductMapper productMapper)
        {
            if (apiProductsProvider == null)
            {
                throw new ArgumentNullException("apiProductsProvider");
            }

            if (productMapper == null)
            {
                throw new ArgumentNullException("productMapper");
            }

            this.apiProductsProvider = apiProductsProvider;
            this.productMapper = productMapper;
        }

        /// <summary>
        /// Get data for product with specified article number from current reseller's shop in Atomia Billing
        /// </summary>
        public CoreProduct GetProduct(string articleNumber)
        {
            var apiProduct = apiProductsProvider.GetProduct(articleNumber);
            var product = this.productMapper.Map(apiProduct);

            return product;
        }
    }
}
