using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atomia.Store.Core.Models;

namespace Atomia.Store.UI.ViewModels
{
    public abstract class DomainsViewModel
    {
        public virtual DomainSearchQuery SearchQuery { get; set; }

        public virtual IList<DomainSearchResult> SearchResults { get; set; }
    }
}
