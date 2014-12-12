using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Models
{
    public class RenewalPeriodInput: RenewalPeriod
    {
        public override int Period { get; set; }

        public override string Unit { get; set; }
    }
}
