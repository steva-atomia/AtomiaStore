using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.PublicBillingApi.Handlers
{
    public abstract class OrderDataHandler
    {
        public abstract PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext);

        protected string Normalize(string orderDataField)
        {
            if (String.IsNullOrEmpty(orderDataField))
            {
                return String.Empty;
            }

            if (String.Compare(orderDataField, "null") == 0 || String.Compare(orderDataField, "NULL") == 0 || String.Compare(orderDataField, "Null") == 0)
            {
                return String.Empty;
            }

            return orderDataField.Trim();
        }

        protected void Add(PublicOrder order, PublicOrderItem item)
        {
            var orderItems = new List<PublicOrderItem>(order.OrderItems);
            
            orderItems.Add(item);

            order.OrderItems = orderItems.ToArray();
        }

        protected void Add(PublicOrder order, PublicOrderCustomData customData)
        {
            var customAttributes = new List<PublicOrderCustomData>(order.CustomData);

            customAttributes.Add(customData);

            order.CustomData = customAttributes.ToArray();
        }

        protected PublicOrderItem ExistingOrderItem(PublicOrder order, ItemData item)
        {
            PublicOrderItem existingOrderItem = null;
            var domainAttr = item.CartItem.CustomAttributes.FirstOrDefault(ca => ca.Name == "DomainName");

            if (domainAttr != null)
            {
                existingOrderItem = order.OrderItems.FirstOrDefault(orderItem =>
                    orderItem.ItemNumber == item.ArticleNumber &&
                    orderItem.RenewalPeriodId == item.RenewalPeriodId &&
                    orderItem.CustomData.Any(x => x.Name == "DomainName" && x.Value == domainAttr.Value));
            }
            else
            {
                existingOrderItem = order.OrderItems.FirstOrDefault(orderItem =>
                    orderItem.ItemNumber == item.ArticleNumber &&
                    orderItem.RenewalPeriodId == item.RenewalPeriodId);
            }

            return existingOrderItem;
        }

        protected bool OrderItemsContain(PublicOrder order, ItemData item)
        {
            return ExistingOrderItem(order, item) != null;
        }
    }
}
