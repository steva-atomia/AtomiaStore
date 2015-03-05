using Atomia.Store.Core;

namespace Atomia.Store.Fakes.Adapters
{
    public sealed class FakeCurrencyPreferenceProvider : ICurrencyPreferenceProvider
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
