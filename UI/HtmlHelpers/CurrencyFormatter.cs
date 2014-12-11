using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.HtmlHelpers
{
    public static class CurrencyFormatter
    {
        /// <summary>
        /// Return default .NET currency format or fallback to simple 
        /// </summary>
        public static MvcHtmlString FormatCurrency(this HtmlHelper htmlHelper, decimal amount, string currencyCode)
        {
            var culture = CultureInfo.GetCultures(CultureTypes.SpecificCultures).First(c => new RegionInfo(c.LCID).ISOCurrencySymbol == currencyCode.ToUpper());

            if (culture == default(CultureInfo))
            {
                return MvcHtmlString.Create(string.Format("{0} {1}", currencyCode, amount));
            }
            else
            {
                return MvcHtmlString.Create(amount.ToString("C2", culture));
            }
        }
    }
}
