using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Linq;

namespace Atomia.Store.PublicOrderHandlers.CartItemHandlers
{
    /// <summary>
    /// <see cref="Atomia.Store.PublicBillingApi.Handlers.OrderDataHandler"> that is used for items in cart that are not handled by any other order data handler.
    /// </summary>
    public class DefaultHandler : OrderDataHandler
    {
        /// <summary>
        /// Amend order with any items that have not been added to order yet.
        /// </summary>
        public override PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext)
        {
            var unprocessedItems = orderContext.ItemData.Where(i => !OrderItemsContain(order, i));

            foreach (var item in unprocessedItems)
            {
                Add(order, new PublicOrderItem
                {
                    ItemId = Guid.Empty,
                    ItemNumber = item.ArticleNumber,
                    RenewalPeriodId = item.RenewalPeriodId,
                    Quantity = item.Quantity
                });
            }

            return order;
        }
    }
}
