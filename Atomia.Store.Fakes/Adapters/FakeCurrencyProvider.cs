using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakeCurrencyProvider : ICurrencyProvider
    {
        public IList<Currency> GetAvailableCurrencies()
        {
            return new List<Currency> { new Currency("USD"), new Currency("SEK") };
        }

        public Currency GetDefaultCurrency()
        {
            return new Currency("USD");
        }
    }
}
