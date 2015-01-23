using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public interface IDomainsProvider
    {
        IEnumerable<Product> GetDomains(ICollection<SearchTerm> terms);

        string GetStatus(string domainName);
    }
}
