using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakeProductsProvider : PackagesProvider
    {
        public override IEnumerable<Product> GetProducts(ProductSearchQuery searchQuery)
        {
            var products = new List<Product>
            {
                new Product
                {
                    Category = "Hosting",
                    ArticleNumber = "DNS-PK",
                    PricingVariants = new List<PricingVariant>
                    {
                        new PricingVariant
                        {
                            Price = 0,
                            RenewalPeriod = new RenewalPeriod() { Period = 1, Unit = "YEAR" }
                        }
                    },
                },
                new Product
                {
                    Category = "Hosting",
                    ArticleNumber = "HST-GLD",
                    PricingVariants = new List<PricingVariant>
                    {
                        new PricingVariant 
                        {
                            Price = 10m,
                            RenewalPeriod = new RenewalPeriod() { Period = 1, Unit = "YEAR" }
                        },
                        new PricingVariant 
                        {
                            Price = 20m,
                            RenewalPeriod = new RenewalPeriod() { Period = 2, Unit = "YEAR" },
                        },
                    },
                    CustomAttributes = new List<CustomAttribute>
                    {
                        new CustomAttribute {
                            Name = "Fizz", 
                            Value = "Buzz"
                        },
                        new CustomAttribute {
                            Name = "Foo", 
                            Value = "Bar" 
                        }
                    }
                },
                new Product
                {
                    Category = "Hosting",
                    ArticleNumber = "HST-PLT",
                    PricingVariants = new List<PricingVariant>
                    {
                        new PricingVariant
                        {
                            Price = 20m,
                            RenewalPeriod = new RenewalPeriod() { Period = 1, Unit = "YEAR" }
                        },
                        new PricingVariant {
                            Price = 40m,
                            RenewalPeriod = new RenewalPeriod() { Period = 2, Unit = "YEAR" }
                        },
                    },
                },
                new Product
                {
                    Category = "Extra service",
                    ArticleNumber = "XSV-MYSQL",
                    PricingVariants = new List<PricingVariant>
                    {
                        new PricingVariant
                        {
                            Price = 10m,
                            RenewalPeriod = new RenewalPeriod() { Period = 1, Unit = "YEAR" }
                        },
                        new PricingVariant
                        {
                            Price = 20m,
                            RenewalPeriod = new RenewalPeriod() { Period = 2, Unit = "YEAR" }
                        },
                    },
                },
                new Product
                {
                    Category = "Extra service",
                    ArticleNumber = "XSV-MSSQL",
                    PricingVariants = new List<PricingVariant>
                    {
                        new PricingVariant
                        {
                            Price = 10m,
                            RenewalPeriod = new RenewalPeriod() { Period = 1, Unit = "YEAR" }
                        },
                        new PricingVariant
                        {
                            Price = 20m,
                            RenewalPeriod = new RenewalPeriod() { Period = 2, Unit = "YEAR" }
                        },
                    },
                }
            };

            return products;
        }
    }
}
