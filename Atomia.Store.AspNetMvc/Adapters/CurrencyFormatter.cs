using Atomia.Store.Core;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Adapters
{
    public class CurrencyFormatter : ICurrencyFormatter
    {
        private readonly ICurrencyPreferenceProvider currencyPreferenceProvider = DependencyResolver.Current.GetService<ICurrencyPreferenceProvider>();

        /// <summary>
        /// Return default .NET currency format or fallback to simple 
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
