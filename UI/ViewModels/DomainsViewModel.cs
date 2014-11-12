using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atomia.OrderPage.Core.Models;

namespace Atomia.OrderPage.UI.ViewModels
{
    public abstract class DomainsViewModel
    {
        public virtual DomainSearchQuery SearchQuery { get; set; }
        public virtual IList<DomainSearchResult> SearchResults { get; set; }
    }
}
