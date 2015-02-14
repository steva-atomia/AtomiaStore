using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakePremiumDomainsProvider : IDomainsProvider
    {
        private static string lastSearchTerm = "";

        public DomainSearchData FindDomains(ICollection<string> searchTerms)
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
                        "com",
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
                        "se",
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
                        "eu",
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
                        "net",
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
                        "info",
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
                        "biz",
                        searchTerm + ".biz",
                        DomainResult.AVAILABLE,
                        1
                    )
                );
            }

            var data = new DomainSearchData
            {
                FinishSearch = false,
                DomainSearchId = 1,
                Results = results
            };

            return data;
        }

        public DomainSearchData CheckStatus(int domainSearchId)
        {
            var data = FindDomains(new List<string>{ lastSearchTerm });
            data.Results = data.Results.Select(r => SetAvailable(r));

            return data;
        }


        public IEnumerable<string> GetDomainCategories()
        {
            return new List<string> { "TLD", "TransferTLD", "OwnDomain" };
        }

        private DomainResult SetAvailable(DomainResult result)
        {
            return new DomainResult(result.Product, result.TLD, result.DomainName, DomainResult.AVAILABLE, result.DomainSearchId);
        }
    }
}
