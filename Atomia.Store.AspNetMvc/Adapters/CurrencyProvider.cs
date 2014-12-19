using Atomia.Store.Core;
using System;
using System.Web;

namespace Atomia.Store.AspNetMvc.Adapters
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
