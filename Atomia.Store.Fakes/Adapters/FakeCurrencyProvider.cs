using Atomia.Store.Core;
using System.Collections.Generic;

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
