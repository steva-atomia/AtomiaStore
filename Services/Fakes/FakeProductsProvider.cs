using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atomia.Store.Core;

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
                products.Add(new Product("HST-GLD", 10m, displayProvider, currencyProvider)
                {
                    RenewalPeriods = new List<RenewalPeriod>
                        {
                            new RenewalPeriod {Period = 1, Unit = "YEAR"},
                            new RenewalPeriod {Period = 2, Unit = "YEAR"},
                        }
                });
                products.Add(new Product("HST-PLT", 10m, displayProvider, currencyProvider)
                {
                    RenewalPeriods = new List<RenewalPeriod>
                        {
                            new RenewalPeriod {Period = 1, Unit = "YEAR"},
                            new RenewalPeriod {Period = 2, Unit = "YEAR"},
                        }
                });
            }
            else if (category == "Extra service")
            {
                products.Add(new Product("XSV-MYSQL", 10m, displayProvider, currencyProvider)
                {
                    RenewalPeriods = new List<RenewalPeriod>
                        {
                            new RenewalPeriod {Period = 1, Unit = "YEAR"}
                        }
                });
                products.Add(new Product("XSV-MSSQL", 10m, displayProvider, currencyProvider)
                {
                    RenewalPeriods = new List<RenewalPeriod>
                        {
                            new RenewalPeriod {Period = 1, Unit = "YEAR"}
                        }
                });
            }

            return products;
        }
    }
}
