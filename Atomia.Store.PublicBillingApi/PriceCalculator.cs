using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System.Collections.Generic;
using System.Linq;
using Atomia.Web.Plugin.ProductsProvider;

namespace Atomia.Store.PublicBillingApi
{
    /// <summary>
    /// Helper to calculate price including or excluding VAT.
    /// </summary>
    public class PriceCalculator
    {
        private readonly bool pricesIncludeVat;

        public PriceCalculator(bool pricesIncludeVat)
        {
            this.pricesIncludeVat = pricesIncludeVat;
        }

        public decimal CalculatePrice(decimal priceExcludingTax, IEnumerable<ProductTax> taxes)
        {
            decimal totalTax = 0;

            if (taxes != null && this.pricesIncludeVat)
            {
                foreach (var tax in taxes)
                {
                    if (tax.ApplyToAmountOnly)
                    {
                        totalTax += priceExcludingTax * (tax.Percent / 100);
                    }
                    else
                    {
                        totalTax += (priceExcludingTax + totalTax) * (tax.Percent / 100);
                    }
                }
            }

            return priceExcludingTax + totalTax;
        }

        public decimal CalculatePrice(decimal priceExcludingTax, IEnumerable<PublicOrderTax> publicOrderTaxes)
        {
            List<ProductTax> taxes = null;

            if (publicOrderTaxes != null)
            {
                taxes = publicOrderTaxes.Select(t => new ProductTax(t)).ToList();
            }

            return CalculatePrice(priceExcludingTax, taxes);
        }
    }
}
