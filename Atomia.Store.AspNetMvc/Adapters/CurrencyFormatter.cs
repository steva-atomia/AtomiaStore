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

        /// <summary>
        /// Format amount based on culture from preferred currency, or fallback to simple currency code prefix.
        /// </summary>
        public string FormatAmount(decimal amount)
        {
            var currencyCode = currencyPreferenceProvider.GetCurrentCurrency().Code;
            var culture = CultureInfo.GetCultures(CultureTypes.SpecificCultures).First(c => new RegionInfo(c.LCID).ISOCurrencySymbol == currencyCode.ToUpper());

            if (culture == default(CultureInfo))
            {
                return string.Format("{0} {1}", currencyCode, amount);
            }
            else
            {
                return amount.ToString("C2", culture);
            }
        }
    }
}
