using Atomia.Store.Core;
using Atomia.Web.Plugin.ProductsProvider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.PublicBillingApi
{
    /// <summary>
    /// Provide Atomia Billing renewal period id for <see cref="Atomia.Store.Core.CartItem">CartItems</see>
    /// </summary>
    public sealed class RenewalPeriodProvider
    {
        private readonly ApiProductsProvider apiProductsProvider;

        /// <summary>
        /// Create new instance 
        /// </summary>
        public RenewalPeriodProvider(ApiProductsProvider apiProductsProvider)
        {
            if (apiProductsProvider == null)
            {
                throw new ArgumentNullException("apiProductsProvider");
            }

            this.apiProductsProvider = apiProductsProvider;
        }

        /// <summary>
        /// Get Atomia Billing renewal period id for <see cref="CartItem"/>
        /// </summary
        public Guid GetRenewalPeriodId(CartItem cartItem)
        {
            if (cartItem.RenewalPeriod == null)
            {
                return Guid.Empty;
            }

            var period = cartItem.RenewalPeriod.Period;
            var unit = cartItem.RenewalPeriod.Unit;
            var product = apiProductsProvider.GetProduct(cartItem.ArticleNumber);

            var apiRenewalPeriod = product.RenewalPeriods
                .FirstOrDefault(r => r.RenewalPeriodUnit.ToUpper() == unit && r.RenewalPeriodValue == period);

            if (apiRenewalPeriod == null)
            {
                throw new InvalidOperationException(String.Format("No renewal period {0} {1} found", period, unit));
            }

            return apiRenewalPeriod.Id;
        }
    }
}
