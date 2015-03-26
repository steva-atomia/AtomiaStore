using System.Collections.Generic;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Get avaiable customer types
    /// </summary>
    public interface ICustomerTypeProvider
    {
        /// <summary>
        /// Get avaiable customer types
        /// </summary>
        IEnumerable<CustomerType> GetCustomerTypes();
    }
}
