
namespace Atomia.Store.Core
{
    /// <summary>
    /// Format a number as a currency
    /// </summary>
    /// <remarks>
    /// Instances must rely on dependecy injection or service location to determine what currency to format the amount as, see <see cref="ICurrencyPreferenceProvider"/>
    /// </remarks>
    public interface ICurrencyFormatter
    {
        /// <summary>
        /// Format the amount to currency string
        /// </summary>
        /// <param name="amount">The amount to format</param>
        /// <returns>A string representing the amount in a currency</returns>
        string FormatAmount(decimal amount);

        /// <summary>
        /// Format the percentage rate to string,
        /// </summary>
        /// <param name="percentage">The percentage rate to format, e.g. 10 for 10%</param>
        /// <returns>A string representing the percentage rate</returns>
        string FormatPercentageRate(decimal percentage);
    }
}
