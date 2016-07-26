// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrencyFilter.cs" company="Atomia AB">
//   Copyright (C) 2015/2016 Atomia AB. All rights reserved
// </copyright>
// <summary>
//   Defines the CurrencyFilter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using System.Web.Mvc;

using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Filters
{
    /// <summary>
    /// Defines the CurrencyFilter type.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ActionFilterAttribute" />
    public class CurrencyFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            if (filterContext.Controller.ControllerContext.IsChildAction || request.IsAjaxRequest())
            {
                return;
            }

            var currencyPreferenceProvider = DependencyResolver.Current.GetService<ICurrencyPreferenceProvider>();
            var currencyProvider = DependencyResolver.Current.GetService<ICurrencyProvider>();

            if (request.QueryString["ccy"] != null)
            {
                string currencyCode = request.QueryString["ccy"];

                Currency currency = currencyProvider.GetAvailableCurrencies().FirstOrDefault(c => c.Code == currencyCode);
                if (currency != default(Currency))
                {
                    currencyPreferenceProvider.SetPreferredCurrency(currency);
                }
            }
        }
    }
}
