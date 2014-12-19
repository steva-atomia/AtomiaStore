using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public interface IProductsProvider
    {
        IEnumerable<Product> GetProducts(ProductSearchQuery searchQuery);
    }
}

