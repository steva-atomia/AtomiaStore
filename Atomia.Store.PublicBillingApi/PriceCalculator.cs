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
        private readonly bool inclusiveTaxCalculationType;

        public PriceCalculator(bool pricesIncludeVat, bool inclusiveTaxCalculationType)
        {
            this.pricesIncludeVat = pricesIncludeVat;
            this.inclusiveTaxCalculationType = inclusiveTaxCalculationType;
        }

        public decimal CalculatePrice(decimal priceExcludingTax, IEnumerable<ProductTax> taxes)
        {
            decimal totalTax = 0;

            if (this.inclusiveTaxCalculationType == false && taxes != null && this.pricesIncludeVat)
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
