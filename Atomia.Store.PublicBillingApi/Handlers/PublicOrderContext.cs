using Atomia.Store.Core;
using System;
using System.Collections.Generic;

namespace Atomia.Store.PublicBillingApi.Handlers
{
    public sealed class PublicOrderContext
    {
        private readonly OrderContext orderContext;
        private readonly List<ItemData> itemData = new List<ItemData>();

        public PublicOrderContext(OrderContext orderContext)
        {
            if (orderContext == null)
            {
                throw new ArgumentNullException("orderContext");
            }

            this.orderContext = orderContext;
        }

        public Cart Cart 
        { 
            get
            {
                return orderContext.Cart; 
            }
        }

        public IEnumerable<ContactData> ContactData
        { 
            get
            {
                return orderContext.ContactData; 
            }
        }

        public PaymentData PaymentData
        { 
            get
            {
                return orderContext.PaymentData; 
            }
        }

        public IEnumerable<ItemData> ItemData
        {
            get
            {
                return itemData;
            }
        }

        public IEnumerable<object> ExtraData
        {
            get
            {
                return orderContext.ExtraData ?? new object[0];
            }
        }

        public void AddItemData(ItemData item)
        {
            if (itemData == null)
            {
                throw new ArgumentNullException("item");
            }

            itemData.Add(item);
        }
    }
}
