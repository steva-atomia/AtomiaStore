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
        private readonly ICurrencyProvider currencyProvider;

        public DomainSearchService(IItemDisplayProvider itemDisplayProvider, ICurrencyProvider currencyProvider)
        {
            this.itemDisplayProvider = itemDisplayProvider;
            this.currencyProvider = currencyProvider;
        }

        public IList<Product> FindDomains(DomainSearchQuery searchQuery) 
        {
            var results = new List<Product>();
            
            if (!string.IsNullOrEmpty(searchQuery.SearchTerm))
            {
                var renewalPeriods = new List<RenewalPeriod> { new RenewalPeriod { Period = 1, Unit = "YEAR" } };
                var customAttributes = new List<CustomAttribute>
                {
                    new CustomAttribute 
                    {
                        Name = "DomainName",
                        Values = new List<string>
                        {
                            searchQuery.SearchTerm + ".com"
                        },
                        RequiredInput = true
                    },
                    new CustomAttribute {
                        Name = "Status",
                        Values = new List<string>
                        {
                            "Available"
                        },
                        RequiredInput = false
                    }
                };

                var product = new Product("DMN-COM", 10m, itemDisplayProvider, currencyProvider)
                {
                    RenewalPeriods = renewalPeriods,
                    CustomAttributes = customAttributes
                };
                results.Add(product);
            }

            return results;
        }
    }
}
