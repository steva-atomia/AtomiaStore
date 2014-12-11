using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atomia.Store.Core;

namespace Atomia.Store.Services.WebPluginDomainSearch
{
    public class DomainSearchService : IDomainSearchService
    {
        private readonly IItemDisplayProvider itemDisplayProvider;

        public DomainSearchService(IItemDisplayProvider itemDisplayProvider)
        {
            this.itemDisplayProvider = itemDisplayProvider;
        }

        public IList<Product> FindDomains(DomainSearchQuery searchQuery) 
        {
            var results = new List<Product>();
            
            if (searchQuery.SearchTerms.Any(term => !string.IsNullOrEmpty(term)))
            {
                results.Add(new Product("DMN-COM", 10m, itemDisplayProvider)
                {
                    RenewalPeriods = new List<RenewalPeriod> { new RenewalPeriod { Period = 1, Unit = "YEAR" } },
                    CustomAttributes = new List<CustomAttribute>
                    {
                        new CustomAttribute 
                        {
                            Name = "DomainName",
                            Values =
                            {
                                searchQuery.SearchTerms.First() + ".com"
                            },
                            RequiredInput = true
                        },
                        new CustomAttribute {
                            Name = "Status",
                            Values =
                            {
                                "Available"
                            },
                            RequiredInput = false
                        }
                    }
                });
            }

            return results;
        }
    }
}
