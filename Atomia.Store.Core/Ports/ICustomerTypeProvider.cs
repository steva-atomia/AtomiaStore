using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public interface ICustomerTypeProvider
    {
        IEnumerable<CustomerType> GetCustomerTypes();
    }
}
