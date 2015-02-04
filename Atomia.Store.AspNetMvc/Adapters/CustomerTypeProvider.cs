using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Adapters
{
    public class CustomerTypeProvider : ICustomerTypeProvider
    {
        private readonly IResourceProvider resourceProvider;

        public CustomerTypeProvider(IResourceProvider resourceProvider)
        {
            this.resourceProvider = resourceProvider;
        }

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
