using System.Collections.Generic;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Get default and available currencies, <see cref="Currency"/>
    /// </summary>
    public interface ICurrencyProvider
    {
        /// <summary>
        /// Get all available currencies, <see cref="Currency"/>
        /// </summary>
        IList<Currency> GetAvailableCurrencies();

        /// <summary>
        /// Get the default <see cref="Currency"/>
        /// </summary>
        Currency GetDefaultCurrency();
    }
}
