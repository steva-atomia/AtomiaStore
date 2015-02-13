using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakeCurrencyPreferenceProvider : ICurrencyPreferenceProvider
    {
        public void SetPreferredCurrency(Currency currency)
        {
            
        }

        public Currency GetCurrentCurrency()
        {
            return new Currency("USD");
        }
    }
}
