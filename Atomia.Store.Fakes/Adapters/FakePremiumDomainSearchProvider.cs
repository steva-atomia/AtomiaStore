using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakePremiumDomainSearchProvider : IDomainsProvider
    {
        public IEnumerable<Product> GetDomains(ICollection<SearchTerm> terms)
        {
            var results = new List<Product>();
            var searchTerm = terms.First().Value;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var renewalPeriods = new List<RenewalPeriod> { new RenewalPeriod(1, RenewalPeriod.YEAR) };
                results.Add(new Product
                {
                    ArticleNumber = "DMN-COM",
                    PricingVariants = renewalPeriods.Select(r => new PricingVariant { Price = 10m, RenewalPeriod = r }).ToList(),
                    CustomAttributes = new List<CustomAttribute>
                    {
                        new CustomAttribute 
                        {
                            Name = "DomainName",
                            Value = searchTerm + ".com"
                        },
                        new CustomAttribute {
                            Name = "Status",
                            Value = "available"
                        },
                        new CustomAttribute {
                            Name = "Premium",
                            Value = "true"
                        }
                    }
                });

                results.Add(new Product
                {
                    ArticleNumber = "DMN-SE",
                    PricingVariants = renewalPeriods.Select(r => new PricingVariant { Price = 10m, RenewalPeriod = r }).ToList(),
                    CustomAttributes = new List<CustomAttribute>
                    {
                        new CustomAttribute 
                        {
                            Name = "DomainName",
                            Value = searchTerm + ".se"
                        },
                        new CustomAttribute {
                            Name = "Status",
                            Value = "available"
                        },
                        new CustomAttribute {
                            Name = "Premium",
                            Value = "true"
                        }
                    }
                });

                results.Add(new Product
                {
                    ArticleNumber = "DMN-EU",
                    PricingVariants = renewalPeriods.Select(r => new PricingVariant { Price = 10m, RenewalPeriod = r }).ToList(),
                    CustomAttributes = new List<CustomAttribute>
                    {
                        new CustomAttribute 
                        {
                            Name = "DomainName",
                            Value = searchTerm + ".eu"
                        },
                        new CustomAttribute {
                            Name = "Status",
                            Value = "available"
                        },
                        new CustomAttribute {
                            Name = "Premium",
                            Value = "true"
                        }
                    }
                });

                results.Add(new Product
                {
                    ArticleNumber = "DMN-NET",
                    PricingVariants = renewalPeriods.Select(r => new PricingVariant { Price = 10m, RenewalPeriod = r }).ToList(),
                    CustomAttributes = new List<CustomAttribute>
                    {
                        new CustomAttribute 
                        {
                            Name = "DomainName",
                            Value = searchTerm + ".net"
                        },
                        new CustomAttribute {
                            Name = "Status",
                            Value = "available"
                        }
                    }
                });

                results.Add(new Product
                {
                    ArticleNumber = "DMN-INFO",
                    PricingVariants = renewalPeriods.Select(r => new PricingVariant { Price = 10m, RenewalPeriod = r }).ToList(),
                    CustomAttributes = new List<CustomAttribute>
                    {
                        new CustomAttribute 
                        {
                            Name = "DomainName",
                            Value = searchTerm + ".info"
                        },
                        new CustomAttribute {
                            Name = "Status",
                            Value = "processing"
                        }
                    }
                });

                results.Add(new Product
                {
                    ArticleNumber = "DMN-BIZ",
                    PricingVariants = renewalPeriods.Select(r => new PricingVariant { Price = 10m, RenewalPeriod = r }).ToList(),
                    CustomAttributes = new List<CustomAttribute>
                    {
                        new CustomAttribute 
                        {
                            Name = "DomainName",
                            Value = searchTerm + ".biz"
                        },
                        new CustomAttribute {
                            Name = "Status",
                            Value = "unavailable"
                        }
                    }
                });

                results.Add(new Product
                {
                    ArticleNumber = "DMN-RS",
                    PricingVariants = renewalPeriods.Select(r => new PricingVariant { Price = 10m, RenewalPeriod = r }).ToList(),
                    CustomAttributes = new List<CustomAttribute>
                    {
                        new CustomAttribute 
                        {
                            Name = "DomainName",
                            Value = searchTerm + ".rs"
                        },
                        new CustomAttribute {
                            Name = "Status",
                            Value = "unavailable"
                        }
                    }
                });
            }

            return results;
        }

        public string GetStatus(string domainName)
        {
            return "available";
        }
    }
}
