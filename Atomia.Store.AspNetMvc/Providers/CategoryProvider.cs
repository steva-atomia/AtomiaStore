using Atomia.Store.Core;
using System.Collections.Generic;

namespace Atomia.Store.AspNetMvc.Providers
{
    public abstract class CategoryProvider : IProductsProvider
    {
        public abstract IEnumerable<Product> GetProducts(ProductSearchQuery searchQuery);
    }
}
