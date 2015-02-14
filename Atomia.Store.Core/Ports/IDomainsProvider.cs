using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public interface IDomainsProvider
    {
        IEnumerable<DomainResult> FindDomains(ICollection<string> searchTerms);

        IEnumerable<DomainResult> CheckStatus(int domainSearchId);

        IEnumerable<string> GetDomainCategories();
    }
}
