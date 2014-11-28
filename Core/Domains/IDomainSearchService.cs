using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atomia.Store.Core.Domains
{
    public interface IDomainSearchService
    {
        IList<DomainSearchResult> FindDomains(DomainSearchQuery query);
    }
}
