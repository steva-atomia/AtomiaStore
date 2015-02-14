using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public interface IDomainsProvider
    {
        DomainSearchData FindDomains(ICollection<string> searchTerms);

        DomainSearchData CheckStatus(int domainSearchId);

        IEnumerable<string> GetDomainCategories();
    }
}
