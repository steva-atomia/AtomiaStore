using Atomia.Store.Core;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// View model of <see cref="Atomia.Store.Core.PricingVariant"/> with values formatted and localized for display
    /// </summary>
    public class PricingVariantModel
    {
        private readonly ICurrencyFormatter currencyFormatter = DependencyResolver.Current.GetService<ICurrencyFormatter>();
        private readonly IResourceProvider resourceProvider = DependencyResolver.Current.GetService<IResourceProvider>();
        private readonly PricingVariant pricingVariant;

        /// <summary>
        /// Construct an instance from a <see cref="Atomia.Store.Core.PricingVariant"/>
        /// </summary>
        public PricingVariantModel(PricingVariant pricingVariant)
        {
            this.pricingVariant = pricingVariant;
        }

        /// <summary>
        /// Display formatted and localized renewal period
        /// </summary>
        public RenewalPeriodModel RenewalPeriod
        {
            get
            {
                if (pricingVariant.RenewalPeriod != null)
                {
                    return new RenewalPeriodModel
                    {
                        Period = pricingVariant.RenewalPeriod.Period,
                        Unit = pricingVariant.RenewalPeriod.Unit
                    };
                }

                return null;
            }
        }

        /// <summary>
        /// Currency formatted price.
        /// </summary>
        public string Price
        {
            get
            {
                return currencyFormatter.FormatAmount(pricingVariant.Price);
            }
        }

        /// <summary>
        /// Property wrapper of ToString representation that will be automatically serialized by default JSON serializer.
        /// </summary>
        public string Display
        {
            get
            {
                return ToString();
            }
        }

        /// <summary>
        /// Display formatted and localized representation of this pricing variant.
        /// </summary>
        public override string ToString()
        {
            if (RenewalPeriod != null)
            {
                var renewalPeriodUnitType = RenewalPeriod.Unit;

                if (RenewalPeriod.Period > 1)
                {
                    renewalPeriodUnitType = RenewalPeriod.Unit + "Plural";
                }

                var renewalPeriodUnit = resourceProvider.GetResource(renewalPeriodUnitType);

                return string.Format(resourceProvider.GetResource("PricingVariantDisplay"), Price, RenewalPeriod.Period, renewalPeriodUnit);
            }

            return Price;
        }
    }
}
