using System.Collections.Generic;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Interface for listing products, e.g. in GUI.
    /// </summary>
    public interface IProductListProvider
    {
        /// <summary>
        /// The unique name of the product list provider.
        /// </summary>
        /// <remarks>
        /// Can be used to select among different providers.
        /// </remarks>
        string Name { get; }

        /// <summary>
        /// Get list of products based on a collection of search terms. <seealso cref="SearchTerm"/>
        /// </summary>
        /// <param name="terms">The search terms to use for listing the products.</param>
        /// <returns>The listing of products. <seealso cref="Product"/></returns>
        IEnumerable<Product> GetProducts(ICollection<SearchTerm> terms);
    }
}
