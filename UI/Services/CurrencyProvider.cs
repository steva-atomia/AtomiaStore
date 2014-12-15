using System;
using System.Globalization;
using System.Linq;
using System.Web;
using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Services
{
    public class CurrencyProvider : ICurrencyProvider
    {
        private string currencyCode;

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
    }
}
