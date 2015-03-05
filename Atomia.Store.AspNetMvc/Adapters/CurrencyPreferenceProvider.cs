using Atomia.Store.Core;
using System;
using System.Web;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Adapters
{
    public sealed class CurrencyPreferenceProvider : ICurrencyPreferenceProvider
    {
        private readonly ICurrencyProvider currencyProvider = DependencyResolver.Current.GetService<ICurrencyProvider>();

        public void SetPreferredCurrency(Currency currency)
        {
            if (currency == null)
            {
                throw new ArgumentNullException("currency");
            }

            HttpContext.Current.Session["Currency"] = currency;
        }

        public Currency GetCurrentCurrency()
        {
            var currency = HttpContext.Current.Session["Currency"] as Currency;

            if (currency == null)
            {
                currency = currencyProvider.GetDefaultCurrency();
            }

            return currency;
        }
    }
}
