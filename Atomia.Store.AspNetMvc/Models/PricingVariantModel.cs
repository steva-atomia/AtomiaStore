using Atomia.Store.Core;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Models
{
    public class PricingVariantModel
    {
        private readonly ICurrencyFormatter currencyFormatter;
        private readonly IResourceProvider resourceProvider;
        private readonly PricingVariant pricingVariant;

        public PricingVariantModel(PricingVariant pricingVariant)
        {
            this.currencyFormatter = DependencyResolver.Current.GetService<ICurrencyFormatter>();
            this.resourceProvider = DependencyResolver.Current.GetService<IResourceProvider>();

            this.pricingVariant = pricingVariant;
        }

        public RenewalPeriodModel RenewalPeriod
        {
            get
            {
                return new RenewalPeriodModel
                {
                    Period = pricingVariant.RenewalPeriod.Period,
                    Unit = pricingVariant.RenewalPeriod.Unit
                };
            }
        }

        public string Price
        {
            get
            {
                return currencyFormatter.FormatAmount(pricingVariant.Price);
            }
        }

        public string Display
        {
            get
            {
                return ToString();
            }
        }

        public override string ToString()
        {
            var renewalPeriodUnitType = RenewalPeriod.Unit;

            if (RenewalPeriod.Period > 1)
            {
                renewalPeriodUnitType = RenewalPeriod.Unit + "Plural";
            }

            var renewalPeriodUnit = resourceProvider.GetResource(renewalPeriodUnitType);

            return string.Format(resourceProvider.GetResource("PricingVariantDisplay"), Price, RenewalPeriod.Period, renewalPeriodUnit);
        }
    }
}
