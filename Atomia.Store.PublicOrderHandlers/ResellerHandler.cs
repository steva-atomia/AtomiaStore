using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;

namespace Atomia.Store.PublicOrderHandlers
{
    /// <summary>
    /// Handler to set order reseller id from current reseller
    /// </summary>
    public class ResellerHandler : OrderDataHandler
    {
        private readonly IResellerProvider resellerProvider;

        /// <summary>
        /// Create new instance with access to current reseller.
        /// </summary>
        public ResellerHandler(IResellerProvider resellerProvider)
        {
            if (resellerProvider == null)
            {
                throw new ArgumentNullException("resellerProvider");
            }

            this.resellerProvider = resellerProvider;
        }

        /// <summary>
        /// Set order reseller id from current reseller
        /// </summary>
        public override PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext)
        {
            var resellerId = resellerProvider.GetReseller().Id;

            order.ResellerId = resellerId;

            return order;
        }
    }
}
