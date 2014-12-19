using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Fakes
{
    public class DomainSearchProductsProvider : IProductsProvider
    {
        public IEnumerable<Product> GetProducts(ProductSearchQuery searchQuery) 
        {
            var results = new List<Product>();
            var searchTerm = searchQuery.Terms.First().Value;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var renewalPeriods = new List<RenewalPeriod> { new RenewalPeriod { Period = 1, Unit = "YEAR" } };
                var customAttributes = new List<CustomAttribute>
                {
                    new CustomAttribute 
                    {
                        Name = "DomainName",
                        Value = searchTerm + ".com"
                    },
                    new CustomAttribute {
                        Name = "Status",
                        Value = "Available"
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
