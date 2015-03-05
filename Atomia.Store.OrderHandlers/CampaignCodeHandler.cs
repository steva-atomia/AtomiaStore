using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;

namespace Atomia.Store.PublicOrderHandlers
{
    public class CampaignCodeHandler : OrderDataHandler
    {
        public override PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext)
        {
            if (!String.IsNullOrEmpty(orderContext.Cart.CampaignCode))
            {
                Add(order, new PublicOrderCustomData
                {
                    Name = "CampaignCode",
                    Value = orderContext.Cart.CampaignCode
                });
            }

            return order;
        }
    }
}
