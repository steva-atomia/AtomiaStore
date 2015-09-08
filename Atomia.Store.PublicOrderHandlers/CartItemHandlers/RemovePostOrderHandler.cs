using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.PublicOrderHandlers.CartItemHandlers
{
    /// <summary>
    /// Handler to remove any item with category "PostOrder" from order items.
    /// </summary>
    /// <remarks>This should be the last order handler to amend the order.</remarks>
    public class RemovePostOrderHandler : OrderDataHandler
    {
        /// <summary>
        /// Handle items with category "PostOrder"
        /// </summary>
        public virtual IEnumerable<string> HandledCategories
        {
            get { return new[] { "PostOrder" }; }
        }

        /// <summary>
        /// Remove any item with category "PostOrder" from order items.
        /// </summary>
        public override PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext)
        {
            var postOrderItems = orderContext.ItemData.Where(i => this.HandledCategories.Intersect(i.Categories.Select(c => c.Name)).Count() > 0);
            var orderItems = new List<PublicOrderItem>(order.OrderItems);

            foreach (var postOrderItem in postOrderItems)
            {
                var existingItem = ExistingOrderItem(order, postOrderItem);

                if (existingItem != null)
                {
                    orderItems.Remove(existingItem);
                }
            }

            order.OrderItems = orderItems.ToArray();

            return order;
        }
    }
}
