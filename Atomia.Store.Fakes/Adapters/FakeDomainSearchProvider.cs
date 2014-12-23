using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakeDomainSearchProvider : DomainSearchProvider
    {
        public override IEnumerable<Product> GetProducts(ProductSearchQuery searchQuery) 
        {
            var results = new List<Product>();
            var searchTerm = searchQuery.Terms.First().Value;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var renewalPeriods = new List<RenewalPeriod> { new RenewalPeriod { Period = 1, Unit = "YEAR" } };

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
                            Value = "unavailable"
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
                            Value = "available"
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
                            Value = "available"
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
    }
}
