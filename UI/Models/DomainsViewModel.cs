using Atomia.Store.Core;
using System.Collections.Generic;

namespace Atomia.Store.AspNetMvc.Models
{
    public abstract class DomainsViewModel
    {
        public virtual DomainSearchQuery SearchQuery { get; set; }

        public virtual IList<ProductModel> SearchResults { get; set; }
    }
}
