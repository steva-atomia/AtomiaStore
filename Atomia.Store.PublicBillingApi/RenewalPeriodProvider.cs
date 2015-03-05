using Atomia.Store.Core;
using Atomia.Web.Plugin.ProductsProvider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.PublicBillingApi
{
    public class RenewalPeriodProvider
    {
        private readonly IProductsProvider productsProvider;
        private readonly IResellerProvider resellerProvider;

        public RenewalPeriodProvider(IProductsProvider productsProvider, IResellerProvider resellerProvider)
        {
            if (productsProvider == null)
            {
                throw new ArgumentNullException("productsProvider");
            }

            if (resellerProvider == null)
            {
                throw new ArgumentNullException("resellerProvider");
            }

            this.productsProvider = productsProvider;
            this.resellerProvider = resellerProvider;
        }

        public Guid GetRenewalPeriodId(CartItem cartItem)
        {
            if (cartItem.RenewalPeriod == null)
            {
                return Guid.Empty;
            }

            var period = cartItem.RenewalPeriod.Period;
            var unit = cartItem.RenewalPeriod.Unit;
            var product = productsProvider.GetShopProductsByArticleNumbers(resellerProvider.GetReseller().Id, "", new List<string> { cartItem.ArticleNumber }).FirstOrDefault();

            if (product == null)
            {
                throw new InvalidOperationException(String.Format("No product with articlenumber {0} found", cartItem.ArticleNumber));
            }

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
