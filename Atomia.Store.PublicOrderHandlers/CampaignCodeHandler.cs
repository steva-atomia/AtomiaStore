using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;

namespace Atomia.Store.PublicOrderHandlers
{
    /// <summary>
    /// Handler to amend order with "CampaignCode" custom attribute
    /// </summary>
    public class CampaignCodeHandler : OrderDataHandler
    {
        /// <summary>
        /// Amend order with "CampaignCode" custom attribute
        /// </summary>
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
