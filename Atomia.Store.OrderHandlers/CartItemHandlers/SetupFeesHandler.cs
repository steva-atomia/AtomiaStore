using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using Atomia.Store.Core;

namespace Atomia.Store.PublicOrderHandlers.CartItemHandlers
{
    public class SetupFeesHandler : OrderDataHandler
    {
        public virtual IEnumerable<string> HandledCategories
        {
            get { return new[] { "SetupFee" }; }
        }

        public override PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext)
        {
            var setupFeeItems = orderContext.ItemData.Where(i => this.HandledCategories.Contains(i.Category));

            foreach (var item in setupFeeItems)
            {
                Add(order, new PublicOrderItem
                {
                    ItemId = Guid.Empty,
                    ItemNumber = item.ArticleNumber,
                    RenewalPeriodId = item.RenewalPeriodId,
                    Quantity = 1
                });
            }

            return order;
        }
    }
}
