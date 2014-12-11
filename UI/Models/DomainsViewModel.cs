using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.ViewModels
{
    public abstract class DomainsViewModel
    {
        public virtual DomainSearchQuery SearchQuery { get; set; }

        public virtual IList<Product> SearchResults { get; set; }
    }
}
