using Atomia.Store.Core;
using System;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    /// <summary>
    /// Provide current active <see cref="Reseller"/> from Atomia Billing
    /// </summary>
    public sealed class ResellerProvider : IResellerProvider
    {
        private readonly IResellerDataProvider resellerDataProvider;

        /// <summary>
        /// Create a new instance
        /// </summary>
        public ResellerProvider(IResellerDataProvider resellerDataProvider)
        {
            if (resellerDataProvider == null)
            {
                throw new ArgumentNullException("resellerDataProvider");
            }

            this.resellerDataProvider = resellerDataProvider;
        }

        /// <summary>
        /// Get current reseller from Atomia Billing.
        /// </summary
        public Reseller GetReseller()
        {
            var resellerData =  resellerDataProvider.GetResellerAccountData();
            var defaultResellerData = resellerDataProvider.GetDefaultResellerAccountData();

            var reseller = new Reseller
            {
                Id = resellerData.Id,
                IsSubReseller = resellerData.Id != defaultResellerData.Id,
                InclusiveTaxCalculationType = resellerData.InclusiveTaxCalculationType
            };

            return reseller;
        }
    }
}
