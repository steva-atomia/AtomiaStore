using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atomia.OrderPage.Core.Models;
using Atomia.OrderPage.Core.Services;

namespace Atomia.OrderPage.Services.WebPluginDomainSearch
{
    public class DomainSearchService : IDomainSearchService
    {
        public IList<DomainSearchResult> FindDomains(DomainSearchQuery searchQuery) 
        {
            var results = new List<DomainSearchResult>();
            
            foreach (var term in searchQuery.SearchTerms)
            {
                results.Add(new DomainSearchResult { CurrencyCode = "USD", DomainName = term + ".com", Price = 11.41m });
                results.Add(new DomainSearchResult { CurrencyCode = "USD", DomainName = term + ".net", Price = 11.41m });
                results.Add(new DomainSearchResult { CurrencyCode = "USD", DomainName = term + ".org", Price = 11.41m });
                results.Add(new DomainSearchResult { CurrencyCode = "USD", DomainName = term + ".info", Price = 11.41m });
            }

            return results;
        }
    }
}
