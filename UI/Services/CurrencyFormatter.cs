using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;
using System.Globalization;

namespace Atomia.Store.AspNetMvc.Services
{
    public class CurrencyFormatter : ICurrencyFormatter
    {
        private readonly ICurrencyProvider currencyProvider;

        public CurrencyFormatter(ICurrencyProvider currencyProvider)
        {
            if (currencyProvider == null)
            {
                throw new ArgumentNullException("currencyProvider");
            }

            this.currencyProvider = currencyProvider;
        }

        /// <summary>
        /// Return default .NET currency format or fallback to simple 
        /// </summary>
        public string FormatAmount(decimal amount)
        {
            var currencyCode = currencyProvider.GetCurrencyCode();
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
