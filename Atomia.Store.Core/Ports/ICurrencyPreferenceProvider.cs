
namespace Atomia.Store.Core
{
    /// <summary>
    /// Get and set user's currency preference.
    /// </summary>
    public interface ICurrencyPreferenceProvider
    {
        /// <summary>
        /// Set the user's preferred currency
        /// </summary>
        /// <param name="currency">The currency to set</param>
        void SetPreferredCurrency(Currency currency);

        /// <summary>
        /// Get the user's preferred currency
        /// </summary>
        Currency GetCurrentCurrency();
    }
}
