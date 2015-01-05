using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Providers
{
    public abstract class DomainsProvider : IProductsProvider
    {
        public abstract IEnumerable<Product> GetProducts(ProductSearchQuery searchQuery);
    }
}
