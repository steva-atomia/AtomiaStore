using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public interface IProductsProvider
    {
        string Name { get; }

        IEnumerable<Product> GetProducts(ICollection<SearchTerm> terms);

        Product GetProduct(string articleNumber);
    }
}
