using Atomia.Store.Core;
using System;
using System.Collections.Generic;

namespace Atomia.Store.PublicBillingApi.Handlers
{
    /// <summary>
    /// Collection of different types of data that is needed to place an order via Atomia Billing Public Service.
    /// </summary>
    public sealed class PublicOrderContext
    {
        private readonly OrderContext orderContext;
        private readonly List<ItemData> itemData = new List<ItemData>();

        /// <summary>
        /// Create new instance with the basic order data collected from customer.
        /// </summary>
        public PublicOrderContext(OrderContext orderContext)
        {
            if (orderContext == null)
            {
                throw new ArgumentNullException("orderContext");
            }

            this.orderContext = orderContext;
        }

        /// <summary>
        /// The customer's cart
        /// </summary>
        public Cart Cart 
        { 
            get
            {
                return orderContext.Cart; 
            }
        }

        /// <summary>
        /// The contact data collected from customer
        /// </summary>
        public IEnumerable<ContactData> ContactData
        { 
            get
            {
                return orderContext.ContactData; 
            }
        }

        /// <summary>
        /// The payment data collected from customer
        /// </summary>
        public PaymentData PaymentData
        { 
            get
            {
                return orderContext.PaymentData; 
            }
        }

        /// <summary>
        /// Extended item data for items in customer's cart
        /// </summary>
        public IEnumerable<ItemData> ItemData
        {
            get
            {
                return itemData;
            }
        }

        /// <summary>
        /// Any extra data collected from customer
        /// </summary>
        public IEnumerable<object> ExtraData
        {
            get
            {
                return orderContext.ExtraData ?? new object[0];
            }
        }

        /// <summary>
        /// Add extended item
        /// </summary>
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
