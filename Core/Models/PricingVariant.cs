using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core
{
    public sealed class PricingVariant
    {
        public RenewalPeriod RenewalPeriod { get; set; }

        public decimal Price { get; set; }
    }
}
