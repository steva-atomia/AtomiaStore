using Atomia.Store.Core;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Adapters
{
    /// <summary>
    /// <see cref="Atomia.Store.Core.ICustomerTypeProvider"/> that supports "individual" or "company"
    /// </summary>
    public sealed class CustomerTypeProvider : ICustomerTypeProvider
    {
        private readonly IResourceProvider resourceProvider = DependencyResolver.Current.GetService<IResourceProvider>();

        /// <inheritdoc />
        public IEnumerable<CustomerType> GetCustomerTypes()
        {
            return new List<CustomerType> 
            {
                new CustomerType()
                {
                    Name = resourceProvider.GetResource("Individual"),
                    Id = "individual"
                },
                new CustomerType()
                {
                    Name = resourceProvider.GetResource("Company"),
                    Id = "company"
                }
            };
        }
    }
}
