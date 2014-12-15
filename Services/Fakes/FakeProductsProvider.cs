using Atomia.Store.Core;
using System.Collections.Generic;

namespace Atomia.Store.Services.Fakes
{
    public class FakeProductsProvider : IProductsProvider
    {
        public IList<Product> GetProducts(string category)
        {
            var products = new List<Product>();

            if (category == "Hosting")
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
                            Values = new List<string> 
                            { 
                                "spam", 
                                "eggs" 
                            }
                        },
                        new CustomAttribute {
                            Name = "Foo", 
                            Values = new List<string> 
                            { 
                                "Bar" 
                            }
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
            else if (category == "Extra service")
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
