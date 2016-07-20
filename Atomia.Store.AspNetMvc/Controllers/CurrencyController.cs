// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrencyController.cs" company="Atomia AB">
//   Copyright (C) 2010 Atomia AB. All rights reserved
// </copyright>
// <summary>
//   Defines the CurrencyController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;

using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Controllers
{
    /// <summary>
    /// Defines the CurrencyController type.
    /// </summary>
    public class CurrencyController : Controller
    {
        /// <summary>
        /// The currency provider
        /// </summary>
        private readonly ICurrencyProvider currencyProvider = DependencyResolver.Current.GetService<ICurrencyProvider>();
        
        /// <summary>
        /// The currency preference provider
        /// </summary>
        private readonly ICurrencyPreferenceProvider currencyPreferenceProvider = DependencyResolver.Current.GetService<ICurrencyPreferenceProvider>();

        /// <summary>
        /// Gets the currencies.
        /// </summary>
        /// <returns>JSON with the list of available currencies and the current currency.</returns>
        public JsonResult GetCurrencies()
        {
            return
                JsonEnvelope.Success(
                    new
                        {
                            Currencies = this.currencyProvider.GetAvailableCurrencies(),
                            CurrentCurrency = this.currencyPreferenceProvider.GetCurrentCurrency()
                        });
        }
    }
}