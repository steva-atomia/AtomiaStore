using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atomia.Store.Core;

namespace Atomia.Store.Services.WebPluginDomainSearch
{
    public class DomainSearchService : IDomainSearchService
    {
        public IList<Product> FindDomains(DomainSearchQuery searchQuery) 
        {
            var results = new List<Product>();
            
            if (!string.IsNullOrEmpty(searchQuery.SearchTerm))
            {
                results.Add(new DomainProduct {
                    ArticleNumber = "DMN-COM",
                    CurrencyCode = "SEK",
                    Price = 10m,
                    RenewalPeriods = new List<RenewalPeriod> { new RenewalPeriod { Period = 1, Unit = "YEAR" }},
                    DomainName = searchQuery.SearchTerm + ".com",
                    Status = "Available"
                });
            }

            return results;
        }
    }
}
