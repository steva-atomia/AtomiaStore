using System;

namespace Atomia.Store.Core
{
    public interface IProductProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="articleNumber"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Should throw ArgumentException if product with article number is not found.</exception>
        Product GetProduct(string articleNumber);
    }
}
