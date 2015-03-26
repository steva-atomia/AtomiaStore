using System;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Interface for finding a product.
    /// </summary>
    public interface IProductProvider
    {
        /// <summary>
        /// Get a product based on the specified article number.
        /// </summary>
        /// <param name="articleNumber">The article number for which to get the product</param>
        /// <returns>The found <see cref="Product"/></returns>
        /// <exception cref="ArgumentException">Should throw ArgumentException if <see cref="Product"/> with article number is not found.</exception>
        Product GetProduct(string articleNumber);
    }
}
