using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Services.Fakes
{
    public class FakeDomainSearchService : IDomainSearchService
    {
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

                var product = new Product
                {
                    ArticleNumber = "DMN-COM",
                    PricingVariants = renewalPeriods.Select(r => new PricingVariant { Price = 10m, RenewalPeriod = r }).ToList(),
                    CustomAttributes = customAttributes
                };
                results.Add(product);
            }

            return results;
        }
    }
}
