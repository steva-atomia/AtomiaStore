using System;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Atomia.Store.Core
{
    public class CurrencyProvider : ICurrencyProvider
    {
        private string currencyCode;
        private CultureInfo culture;

        public string GetCurrencyCode()
        {
            if (currencyCode == null)
            {
                currencyCode = HttpContext.Current.Session["CurrencyCode"] as string;
                
                if (string.IsNullOrEmpty(currencyCode))
                {
                    throw new InvalidOperationException("Could not get CurrencyCode from session.");
                }
            }

            return currencyCode;
        }

        public void SetCurrencyCode(string currencyCodeToSet)
        {
            if (string.IsNullOrEmpty(currencyCodeToSet))
            {
                throw new ArgumentException("currencyCodeToSet");
            }

            HttpContext.Current.Session["CurrencyCode"] = currencyCodeToSet;
        }

        /// <summary>
        /// Return default .NET currency format or fallback to simple 
        /// </summary>
        public string FormatAmount(decimal amount)
        { 
            if (culture == default(CultureInfo))
            {
                culture = CultureInfo.GetCultures(CultureTypes.SpecificCultures).First(c => new RegionInfo(c.LCID).ISOCurrencySymbol == GetCurrencyCode().ToUpper());
            }

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
