using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Fakes.Adapters
{
    public sealed class FakeCategoryProductsProvider : IProductListProvider, IProductProvider
    {
        public string Name
        {
            get { return "Category"; }
        }

        public Product GetProduct(string articleNumber)
        {
            var allProducts = GetAllProducts();
            return allProducts.FirstOrDefault(p => p.ArticleNumber == articleNumber);
        }

        public IEnumerable<Product> GetProducts(ICollection<SearchTerm> terms)
        {
            var allProducts = GetAllProducts();
            var category = terms.First().Value;

            return allProducts.Where(p => p.Categories.Select(c => c.Name).Contains(category));
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var renewalPeriods = new List<RenewalPeriod> { new RenewalPeriod(1, RenewalPeriod.YEAR) };

            var products = new List<Product>
            {
                new Product
                {
                    Name = "DNS Package",
                    Categories = new List<Category> { new Category { Name = "HostingPackage", Description = "Hosting package" } },
                    ArticleNumber = "DNS-PK",
                    Description = "DNS Package<ul><li>Domain management</li><li>DNS</li></br></br></ul>",
                    PricingVariants = new List<PricingVariant>
                    {
                        new PricingVariant
                        {
                            Price = 0,
                            RenewalPeriod = null
                        }
                    },
                    CustomAttributes = new List<CustomAttribute>
                    {
                        new CustomAttribute {
                            Name = "tos",
                            Value = "defaultTOS"
                        }
                    }
                },
                new Product
                {
                    Name = "Gold Package",
                    Categories = new List<Category> { new Category { Name = "HostingPackage", Description = "Hosting package" } },
                    ArticleNumber = "HST-GLD",
                    Description = "Basic hosting package<ul><li>Web hosting</li><li>Email</li><li>Domain management</li><li>DNS</li></ul>",
                    PricingVariants = new List<PricingVariant>
                    {
                        new PricingVariant 
                        {
                            Price = 10m,
                            RenewalPeriod = new RenewalPeriod(1, RenewalPeriod.YEAR)
                        }
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
                        },
                        new CustomAttribute {
                            Name = "tos",
                            Value = "defaultTOS"
                        }
                    }
                },
                new Product
                {
                    Name = "Platinum package",
                    Categories = new List<Category> { new Category { Name = "HostingPackage", Description = "Hosting package" } },
                    ArticleNumber = "HST-PLT",
                    Description = "Premium hosting package<ul><li>Web hosting</li><li>Email</li><li>Domain management</li><li>DNS</li></ul>",
                    PricingVariants = new List<PricingVariant>
                    {
                        new PricingVariant 
                        {
                            Price = 10m,
                            RenewalPeriod = new RenewalPeriod(6, RenewalPeriod.MONTH)
                        },
                        new PricingVariant
                        {
                            Price = 20m,
                            RenewalPeriod = new RenewalPeriod(1, RenewalPeriod.YEAR)
                        },
                        new PricingVariant {
                            Price = 40m,
                            RenewalPeriod = new RenewalPeriod(2, RenewalPeriod.YEAR)
                        },
                    },
                    CustomAttributes = new List<CustomAttribute>
                    {
                        new CustomAttribute {
                            Name = "tos",
                            Value = "defaultTOS"
                        }
                    }
                },
                new Product
                {
                    Name = "MySQL Database",
                    Categories = new List<Category> { new Category { Name = "HostingPackage", Description = "Hosting package" } },
                    ArticleNumber = "XSV-MYSQL",
                    Description = "MySQL Database addon.",
                    PricingVariants = new List<PricingVariant>
                    {
                        new PricingVariant
                        {
                            Price = 10m,
                            RenewalPeriod = new RenewalPeriod(1, RenewalPeriod.YEAR)
                        },
                        new PricingVariant
                        {
                            Price = 20m,
                            RenewalPeriod = new RenewalPeriod(2, RenewalPeriod.YEAR)
                        },
                    },
                },
                new Product
                {
                    Name = "MSSQL Database",
                    Categories = new List<Category> { new Category { Name = "HostingPackage", Description = "Hosting package" } },
                    ArticleNumber = "XSV-MSSQL",
                    Description = "MSSQL Database addon.",
                    PricingVariants = new List<PricingVariant>
                    {
                        new PricingVariant
                        {
                            Price = 10m,
                            RenewalPeriod = new RenewalPeriod(1, RenewalPeriod.YEAR)
                        },
                        new PricingVariant
                        {
                            Price = 20m,
                            RenewalPeriod = new RenewalPeriod(2, RenewalPeriod.YEAR)
                        },
                    },
                },
                new Product
                {
                    Name = ".com",
                    ArticleNumber = "DMN-COM",
                    Description = "Domain registration .com",
                    PricingVariants = renewalPeriods.Select(r => new PricingVariant { Price = 10m, RenewalPeriod = r }).ToList(),
                    Categories = new List<Category> { new Category { Name = "TLD", Description = "Domain name" } },
                    CustomAttributes = new List<CustomAttribute>
                    {
                        new CustomAttribute {
                            Name = "tos",
                            Value = "TOS_core"
                        },
                        new CustomAttribute {
                            Name = "productvalue",
                            Value = ".com"
                        }
                    }
                }
          };


            var tlds = new[] { "se", "org", "eu", "net", "info", "co.uk" };

            // Uncomment if you need to test with many tlds. Also see FakePremiumDomainsProvider
            /* tlds = new [] {"se", "org", "eu", "net", "info", "de", "co.uk", "fr", "dk", "fi", "es",
            "co", "it", "io", "cloud",  "global", "be", "ca", "mx", "pro", "aero", "asia", "au", "cl", 
            "coop", "my", "sg", "hk", "hu", "jobs", "lv",  "no", "nyc", "pm", "re", "tf", "wf", "yt", 
            "ro", "ru", "nu", "travel" };*/

            foreach(var tld in tlds)
            {
                products.Add(new Product
                {
                    Name = "." + tld,
                    ArticleNumber = "DMN-" + tld.ToUpper(),
                    Description = "Domain registration ." + tld,
                    Categories = new List<Category> { new Category { Name = "TLD", Description = "Domain name" } },
                    PricingVariants = renewalPeriods.Select(r => new PricingVariant { Price = 10m, RenewalPeriod = r }).ToList(),
                    CustomAttributes = new List<CustomAttribute>()
                    {
                        new CustomAttribute {
                            Name = "productvalue",
                            Value = "." + tld
                        }
                    }
                });
            }

            return products.OrderBy(p => p.PricingVariants.Min(v => v.Price));
        }
    }
}
