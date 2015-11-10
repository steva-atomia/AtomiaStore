using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.PublicOrderHandlers
{
    /// <summary>
    /// Handler to amend order with custom attributes from cart
    /// </summary>
    public class OrderAttributesHandler : OrderDataHandler
    {
        /// <summary>
        /// Amend order with custom attributes from cart
        /// </summary>
        public override PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext)
        {
            if (orderContext.Cart.CustomAttributes != null)
            {
                foreach (var attr in orderContext.Cart.CustomAttributes)
                {
                    Add(order, new PublicOrderCustomData
                    {
                        Name = attr.Name,
                        Value = attr.Value
                    });
                }
            }

            return order;
        }
    }
}
