using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atomia.Store.Core.Models;
using Atomia.Store.Core.Services;

namespace Atomia.Store.Services.WebPluginDomainSearch
{
    public class DomainSearchService : IDomainSearchService
    {
        public IList<DomainSearchResult> FindDomains(DomainSearchQuery searchQuery) 
        {
            var results = new List<DomainSearchResult>();
            
            if (!string.IsNullOrEmpty(searchQuery.SearchTerm))
            {
                results.Add(new DomainSearchResult { CurrencyCode = "USD", DomainName = searchQuery.SearchTerm + ".com", Price = 11.41m, Status = "Available" });
                results.Add(new DomainSearchResult { CurrencyCode = "USD", DomainName = searchQuery.SearchTerm + ".net", Price = 11.41m, Status = "Available" });
                results.Add(new DomainSearchResult { CurrencyCode = "USD", DomainName = searchQuery.SearchTerm + ".org", Price = 11.41m, Status = "Available" });
                results.Add(new DomainSearchResult { CurrencyCode = "USD", DomainName = searchQuery.SearchTerm + ".info", Price = 11.41m, Status = "Available" });
            }

            return results;
        }
    }
}
