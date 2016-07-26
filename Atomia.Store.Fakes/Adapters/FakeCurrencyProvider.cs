using Atomia.Store.Core;
using System.Collections.Generic;

namespace Atomia.Store.Fakes.Adapters
{
    public sealed class FakeCurrencyProvider : ICurrencyProvider
    {
        private readonly IResourceProvider resourceProvider;

        public FakeCurrencyProvider(IResourceProvider resourceProvider)
        {
            this.resourceProvider = resourceProvider;
        }

        public IList<Currency> GetAvailableCurrencies()
        {
            return new List<Currency>
                       {
                           Currency.CreateCurrency(resourceProvider, "USD"),
                           Currency.CreateCurrency(resourceProvider, "SEK")
                       };
        }

        public Currency GetDefaultCurrency()
        {
            return Currency.CreateCurrency(resourceProvider, "USD");
        }
    }
}
