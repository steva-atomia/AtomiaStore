using Atomia.Store.Core;

namespace Atomia.Store.Fakes.Adapters
{
    public sealed class FakeCurrencyPreferenceProvider : ICurrencyPreferenceProvider
    {
        private readonly IResourceProvider resourceProvider;

        public FakeCurrencyPreferenceProvider(IResourceProvider resourceProvider)
        {
            this.resourceProvider = resourceProvider;
        }

        public void SetPreferredCurrency(Currency currency)
        {
            
        }

        public Currency GetCurrentCurrency()
        {
            return Currency.CreateCurrency(resourceProvider, "USD");
        }
    }
}
