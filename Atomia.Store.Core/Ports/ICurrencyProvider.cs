using System;
using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public interface ICurrencyProvider
    {
        IList<Currency> GetAvailableCurrencies();

        Currency GetDefaultCurrency();
    }
}
