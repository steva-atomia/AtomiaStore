using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public interface IProductsProvider
    {
        IList<Product> GetProducts(string category);
    }
}
