using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core
{
    public class PricedRenewalPeriod
    {
        private readonly ICurrencyProvider currencyProvider;

        public PricedRenewalPeriod(ICurrencyProvider currencyProvider)
        {
            this.currencyProvider = currencyProvider;
        }

        public RenewalPeriod RenewalPeriod { get; set; }

        public decimal Price { get; set; }

        public string DisplayPrice
        {
            get
            {
                return currencyProvider.FormatAmount(Price);
            }
        }

        public override string ToString()
        {
            if (RenewalPeriod.Period == 1)
            {
                return DisplayPrice + " per " + RenewalPeriod.Unit.ToLower();
            }
            
            return DisplayPrice + " per " + RenewalPeriod.Period + " " + RenewalPeriod.Unit.ToLower() + "s";
        }
    }
}
