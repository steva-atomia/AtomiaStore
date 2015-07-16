using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Fakes.Adapters
{
    public sealed class FakePremiumDomainsProvider : IDomainsProvider
    {
        private static string lastSearchTerm = "";

        public DomainSearchData FindDomains(ICollection<string> searchTerms)
        {
            var results = new List<DomainResult>();
            var searchTerm = searchTerms.First();

            var premiumTlds = new Dictionary<string, string> 
            {
                {"com", DomainResult.AVAILABLE},
                {"org", DomainResult.AVAILABLE},
                {"net", DomainResult.AVAILABLE},
            };

            var secondaryTlds = new Dictionary<string, string> 
            {
                {"se", DomainResult.AVAILABLE},
                {"eu", DomainResult.UNAVAILABLE},
                {"info", DomainResult.UNKNOWN},
                {"co.uk", DomainResult.AVAILABLE},
                
                // Uncomment if you need to test many tlds. Also see FakeCategoryProductsProvider
                /*{"de", DomainResult.AVAILABLE},
                {"fr", DomainResult.UNAVAILABLE},
                {"dk", DomainResult.AVAILABLE},
                {"fi", DomainResult.UNAVAILABLE},
                {"es", DomainResult.AVAILABLE},
                {"co", DomainResult.UNAVAILABLE},
                {"it", DomainResult.AVAILABLE},
                {"io", DomainResult.UNAVAILABLE},
                {"cloud", DomainResult.AVAILABLE},
                {"global", DomainResult.UNAVAILABLE},
                {"be", DomainResult.AVAILABLE},
                {"ca", DomainResult.UNAVAILABLE},
                {"mx", DomainResult.AVAILABLE},
                {"pro", DomainResult.UNAVAILABLE},
                {"aero", DomainResult.AVAILABLE},
                {"asia", DomainResult.UNAVAILABLE},
                {"au", DomainResult.AVAILABLE},
                {"cl", DomainResult.UNAVAILABLE},
                {"coop", DomainResult.AVAILABLE},
                {"my", DomainResult.UNAVAILABLE},
                {"sg", DomainResult.AVAILABLE},
                {"hk", DomainResult.UNAVAILABLE},
                {"hu", DomainResult.AVAILABLE},
                {"jobs", DomainResult.UNAVAILABLE},
                {"lv", DomainResult.AVAILABLE},
                {"no", DomainResult.UNAVAILABLE},
                {"nyc", DomainResult.AVAILABLE},
                {"pm", DomainResult.UNAVAILABLE},
                {"re", DomainResult.AVAILABLE},
                {"tf", DomainResult.UNAVAILABLE},
                {"wf", DomainResult.AVAILABLE},
                {"yt", DomainResult.UNAVAILABLE},
                {"ro", DomainResult.AVAILABLE},
                {"ru", DomainResult.UNAVAILABLE},
                {"nu", DomainResult.AVAILABLE},
                {"travel", DomainResult.UNAVAILABLE}*/
            };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                lastSearchTerm = searchTerm;
                var renewalPeriods = new List<RenewalPeriod> { new RenewalPeriod(1, RenewalPeriod.YEAR) };

                var i = 0;
                foreach (var tld in premiumTlds)
                {
                    var domainResult = new DomainResult(
                        new Product
                        {
                            ArticleNumber = "DMN-" + tld.Key.ToUpper(),
                            PricingVariants = renewalPeriods.Select(r => new PricingVariant { Price = 10m, RenewalPeriod = r }).ToList(),
                            CustomAttributes = new List<CustomAttribute> { 
                                new CustomAttribute { Name = "Premium", Value = "true"} ,
                                new CustomAttribute { Name = "productvalue", Value = "." + tld.Key} ,
                            }
                        },
                        tld.Key,
                        searchTerm + "." + tld.Key,
                        tld.Value,
                        1
                    );

                    domainResult.Order = i++;

                    results.Add(domainResult);
                }

                foreach (var tld in secondaryTlds)
                {
                    var domainResult = new DomainResult(
                        new Product
                        {
                            ArticleNumber = "DMN-" + tld.Key.ToUpper(),
                            PricingVariants = renewalPeriods.Select(r => new PricingVariant { Price = 10m, RenewalPeriod = r }).ToList(),
                            CustomAttributes = new List<CustomAttribute> { 
                                    new CustomAttribute { Name = "productvalue", Value = "." + tld.Key} ,
                                }
                        },
                        tld.Key,
                        searchTerm + "." + tld.Key,
                        tld.Value,
                        1
                    );

                    domainResult.Order = i++;

                    results.Add(domainResult);
                }
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
            data.FinishSearch = true;

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
