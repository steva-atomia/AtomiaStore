using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakeProductsProvider : IProductsProvider
    {
        public IEnumerable<Product> GetProducts(ProductSearchQuery query)
        {
            var products = new List<Product>();
            var categoryterm = query.Terms.FirstOrDefault(t => t.Key == "category");

            if (categoryterm != null && categoryterm.Value == "Hosting")
            {
                products.Add(new Product
                {
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
                });
                products.Add(new Product
                {
                    ArticleNumber = "HST-PLT",
                    PricingVariants = new List<PricingVariant>
                    {
                        new PricingVariant
                        {
                            Price = 10m,
                            RenewalPeriod = new RenewalPeriod() { Period = 1, Unit = "YEAR" }
                        },
                        new PricingVariant {
                            Price = 20m,
                            RenewalPeriod = new RenewalPeriod() { Period = 2, Unit = "YEAR" }
                        },
                    },
                });
            }
            else if (categoryterm != null && categoryterm.Value == "Extra service")
            {
                products.Add(new Product
                {
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
                });
                products.Add(new Product
                {
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
                });
            }

            return products;
        }
    }
}
