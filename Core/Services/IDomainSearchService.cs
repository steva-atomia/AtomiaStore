using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public interface IDomainSearchService
    {
        IList<Product> FindDomains(DomainSearchQuery query);
    }
}
