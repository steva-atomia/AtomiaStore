using Atomia.Store.Core;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Models
{
    public class RenewalPeriodModel
    {
        private IResourceProvider resourceProvider;

        public RenewalPeriodModel()
        {
            resourceProvider = DependencyResolver.Current.GetService<IResourceProvider>();
        }

        //[Required][1+]
        public int Period { get; set; }

        //[Required]
        public string Unit { get; set; }

        public string Display
        {
            get
            {
                return ToString();
            }
        }

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
