using System.Collections.Generic;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Domain search interface
    /// </summary>
    public interface IDomainsProvider
    {
        /// <summary>
        /// Find domains based on search terms
        /// </summary>
        DomainSearchData FindDomains(ICollection<string> searchTerms);

        /// <summary>
        /// Check status of domain results found with <see cref="FindDomains"/>
        /// </summary>
        /// <param name="domainSearchId">Id for the domain search, originating in <see cref="FindDomains"/></param>
        DomainSearchData CheckStatus(int domainSearchId);

        /// <summary>
        /// Get categories for available domain products.
        /// </summary>
        IEnumerable<string> GetDomainCategories();
    }
}
