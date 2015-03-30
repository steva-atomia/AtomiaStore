using Atomia.Store.Core;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// Renewal period view model with localized values.
    /// </summary>
    public class RenewalPeriodModel
    {
        private readonly IResourceProvider resourceProvider = DependencyResolver.Current.GetService<IResourceProvider>();

        /// <summary>
        /// The renewal period length.
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// The renewal period value. MONTH or YEAR.
        /// </summary>
        public string Unit { get; set; }

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
        /// Display formatted and localized representation of this renewal period.
        /// </summary>
        public override string ToString()
        {
            var renewalPeriodUnitType = this.Unit;

            if (this.Period > 1)
            {
                renewalPeriodUnitType = this.Unit + "Plural";
            }

            var renewalPeriodUnit = resourceProvider.GetResource(renewalPeriodUnitType);

            return string.Format(resourceProvider.GetResource("RenewalPeriodDisplay"), this.Period, renewalPeriodUnit);
        }
    }
}
