using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.PublicBillingApi;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.PublicOrderHandlers
{
    public class ResellerHandler : OrderDataHandler
    {
        private readonly IResellerProvider resellerProvider;

        public ResellerHandler(IResellerProvider resellerProvider)
        {
            if (resellerProvider == null)
            {
                throw new ArgumentNullException("resellerProvider");
            }

            this.resellerProvider = resellerProvider;
        }

        public override PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext)
        {
            var resellerId = resellerProvider.GetReseller().Id;

            order.ResellerId = resellerId;

            return order;
        }
    }
}
