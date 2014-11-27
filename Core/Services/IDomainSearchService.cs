using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atomia.Store.Core.Models;

namespace Atomia.Store.Core.Services
{
    public interface IDomainSearchService
    {
        IList<DomainSearchResult> FindDomains(DomainSearchQuery query);
    }
}
