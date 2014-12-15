using Atomia.Store.Core;
using System.Collections.Generic;

namespace Atomia.Store.Services.Fakes
{
    public class FakeProductsProvider : IProductsProvider
    {
        private readonly IItemDisplayProvider displayProvider;
        private readonly ICurrencyProvider currencyProvider;

        public FakeProductsProvider(IItemDisplayProvider itemDisplayProvider, ICurrencyProvider currencyProvider)
        {
            this.displayProvider = itemDisplayProvider;
            this.currencyProvider = currencyProvider;
        }

        public IList<Product> GetProducts(string category)
        {
            var products = new List<Product>();

            if (category == "Hosting")
            {
                products.Add(new Product("HST-GLD", displayProvider, currencyProvider)
                {
                    RenewalPeriodChoices = new List<PricedRenewalPeriod>
                    {
                        new PricedRenewalPeriod(currencyProvider) 
                        {
                            RenewalPeriod = new RenewalPeriod() { Period = 1, Unit = "YEAR" },
                            Price = 10m
                        },
                        new PricedRenewalPeriod(currencyProvider) {
                            RenewalPeriod = new RenewalPeriod() { Period = 2, Unit = "YEAR" },
                            Price = 20m
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
                products.Add(new Product("HST-PLT", displayProvider, currencyProvider)
                {
                    RenewalPeriodChoices = new List<PricedRenewalPeriod>
                    {
                        new PricedRenewalPeriod(currencyProvider) 
                        {
                            RenewalPeriod = new RenewalPeriod() { Period = 1, Unit = "YEAR" },
                            Price = 10m
                        },
                        new PricedRenewalPeriod(currencyProvider) {
                            RenewalPeriod = new RenewalPeriod() { Period = 2, Unit = "YEAR" },
                            Price = 20m
                        },
                    },
                });
            }
            else if (category == "Extra service")
            {
                products.Add(new Product("XSV-MYSQL", displayProvider, currencyProvider)
                {
                    RenewalPeriodChoices = new List<PricedRenewalPeriod>
                    {
                        new PricedRenewalPeriod(currencyProvider) 
                        {
                            RenewalPeriod = new RenewalPeriod() { Period = 1, Unit = "YEAR" },
                            Price = 10m
                        },
                        new PricedRenewalPeriod(currencyProvider) {
                            RenewalPeriod = new RenewalPeriod() { Period = 2, Unit = "YEAR" },
                            Price = 20m
                        },
                    },
                });
                products.Add(new Product("XSV-MSSQL", displayProvider, currencyProvider)
                {
                    RenewalPeriodChoices = new List<PricedRenewalPeriod>
                    {
                        new PricedRenewalPeriod(currencyProvider) 
                        {
                            RenewalPeriod = new RenewalPeriod() { Period = 1, Unit = "YEAR" },
                            Price = 10m
                        },
                        new PricedRenewalPeriod(currencyProvider) {
                            RenewalPeriod = new RenewalPeriod() { Period = 2, Unit = "YEAR" },
                            Price = 20m
                        },
                    },
                });
            }

            return products;
        }
    }
}
