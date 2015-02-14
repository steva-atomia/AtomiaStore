using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakePremiumDomainSearchProvider : IDomainsProvider
    {
        private static string lastSearchTerm = "";

        public IEnumerable<DomainResult> FindDomains(ICollection<string> searchTerms)
        {
            var results = new List<DomainResult>();
            var searchTerm = searchTerms.First();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                lastSearchTerm = searchTerm;
                var renewalPeriods = new List<RenewalPeriod> { new RenewalPeriod(1, RenewalPeriod.YEAR) };
                results.Add(
                    new DomainResult(
                        new Product
                        {
                            ArticleNumber = "DMN-COM",
                            PricingVariants = renewalPeriods.Select(r => new PricingVariant { Price = 10m, RenewalPeriod = r }).ToList(),
                            CustomAttributes = new List<CustomAttribute> { new CustomAttribute { Name = "Premium", Value = "true"} }
                        },
                        searchTerm + ".com",
                        DomainResult.AVAILABLE,
                        1
                    )
                );

                results.Add(
                    new DomainResult(
                        new Product
                        {
                            ArticleNumber = "DMN-SE",
                            PricingVariants = renewalPeriods.Select(r => new PricingVariant { Price = 10m, RenewalPeriod = r }).ToList(),
                            CustomAttributes = new List<CustomAttribute> { new CustomAttribute { Name = "Premium", Value = "true" } }
                        },
                        searchTerm + ".se",
                        DomainResult.AVAILABLE,
                        1
                    )
                );

                results.Add(
                    new DomainResult(
                        new Product
                        {
                            ArticleNumber = "DMN-EU",
                            PricingVariants = renewalPeriods.Select(r => new PricingVariant { Price = 10m, RenewalPeriod = r }).ToList(),
                            CustomAttributes = new List<CustomAttribute> { new CustomAttribute { Name = "Premium", Value = "true"} }
                        },
                        searchTerm + ".eu",
                        DomainResult.UNAVAILABLE,
                        1
                    )
                );

                results.Add(
                    new DomainResult(
                        new Product
                        {
                            ArticleNumber = "DMN-NET",
                            PricingVariants = renewalPeriods.Select(r => new PricingVariant { Price = 10m, RenewalPeriod = r }).ToList(),
                        },
                        searchTerm + ".net",
                        DomainResult.LOADING,
                        1
                    )
                );

                results.Add(
                    new DomainResult(
                        new Product
                        {
                            ArticleNumber = "DMN-INFO",
                            PricingVariants = renewalPeriods.Select(r => new PricingVariant { Price = 10m, RenewalPeriod = r }).ToList(),
                        },
                        searchTerm + ".info",
                        DomainResult.UNKNOWN,
                        1
                    )
                );

                results.Add(
                    new DomainResult(
                        new Product
                        {
                            ArticleNumber = "DMN-BIZ",
                            PricingVariants = renewalPeriods.Select(r => new PricingVariant { Price = 10m, RenewalPeriod = r }).ToList(),
                        },
                        searchTerm + ".biz",
                        DomainResult.AVAILABLE,
                        1
                    )
                );
            }

            return results;
        }

        public IEnumerable<DomainResult> CheckStatus(int domainSearchId)
        {
            return FindDomains(new List<string>{ lastSearchTerm }).Select(r => SetAvailable(r));
        }


        public IEnumerable<string> GetDomainCategories()
        {
            return new List<string> { "TLD", "TransferTLD", "OwnDomain" };
        }

        private DomainResult SetAvailable(DomainResult result)
        {

            return new DomainResult(result.Product, result.DomainName, DomainResult.AVAILABLE, result.DomainSearchId);
        }
    }
}
