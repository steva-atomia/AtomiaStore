using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public interface IProductListProvider
    {
        string Name { get; }

        IEnumerable<Product> GetProducts(ICollection<SearchTerm> terms);
    }
}
