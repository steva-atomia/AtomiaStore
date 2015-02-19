using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.PublicBillingApi;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.PublicOrderHandlers.CartItemHandlers
{
    public class DefaultHandler : OrderDataHandler
    {
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
