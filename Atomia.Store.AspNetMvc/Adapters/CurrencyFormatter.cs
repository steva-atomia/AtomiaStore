using Atomia.Store.Core;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Adapters
{
    /// <summary>
    /// <see cref="Atomia.Store.Core.ICurrencyFormatter"/> that formats amount based on culture from customer's currency preference.
    /// </summary>
    public sealed class CurrencyFormatter : ICurrencyFormatter
    {
        private readonly ICurrencyPreferenceProvider currencyPreferenceProvider = DependencyResolver.Current.GetService<ICurrencyPreferenceProvider>();
        private readonly string currencyCode;
        private readonly CultureInfo culture;

        public CurrencyFormatter()
        {
            this.currencyCode = currencyPreferenceProvider.GetCurrentCurrency().Code;
            this.culture = CultureInfo.GetCultures(CultureTypes.SpecificCultures).First(c => new RegionInfo(c.LCID).ISOCurrencySymbol == currencyCode.ToUpper());
        }

        /// <summary>
        /// Format amount based on culture from preferred currency, or fallback to simple currency code prefix.
        /// </summary>
        public string FormatAmount(decimal amount)
        {
            if (this.culture == default(CultureInfo))
            {
                return string.Format("{0} {1}", this.currencyCode, amount);
            }
            else
            {
                return amount.ToString("C2", this.culture);
            }
        }

        /// <summary>
        /// Format percentage based on currency culture with decimals only if they are not 0
        /// </summary>
        /// <param name="percentage">The percentage to format, e.g. 10 for 10%</param>
        public string FormatPercentageRate(decimal percentage)
        {
            var percent = percentage / 100;

            if (this.culture == default(CultureInfo))
            {
                return string.Format("{0:#0.## %}", CultureInfo.InvariantCulture);
            }
            else
            {
                return percent.ToString("#0.## %", this.culture);
            }
        }
    }
}
