using System;
using System.Collections.Generic;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Data collected from which an order can be created.
    /// </summary>
    public sealed class OrderContext
    {
        private readonly Cart cart;
        private readonly IEnumerable<ContactData> contactData;
        private readonly PaymentData paymentData;
        private readonly IEnumerable<object> extraData;

        /// <summary>
        /// OrderContext constructor
        /// </summary>
        /// <param name="cart">Collected cart data</param>
        /// <param name="contactDataCollection">Collected contact data</param>
        /// <param name="paymentData">Collected payment data</param>
        /// <param name="extraData">Any unspecified extra data</param>
        public OrderContext(Cart cart, IContactDataCollection contactDataCollection, PaymentData paymentData, IEnumerable<object> extraData)
        {
            if (cart == null)
            {
                throw new ArgumentNullException("cart");
            }

            if (contactDataCollection == null)
            {
                throw new ArgumentNullException("contactDataCollection");
            }

            if (paymentData == null)
            {
                throw new ArgumentNullException("paymentData");
            }

            this.cart = cart;
            this.contactData = contactDataCollection.GetContactData();
            this.paymentData = paymentData;
            this.extraData = extraData;
        }

        /// <summary>
        /// The collected <see cref="Cart"/> data
        /// </summary>
        public Cart Cart
        {
            get 
            { 
                return cart; 
            }
        }

        /// <summary>
        /// The collected <see cref="ContactData"/>
        /// </summary>
        public IEnumerable<ContactData> ContactData
        {
            get
            {
                return contactData;
            }
        }

        /// <summary>
        /// The collected <see cref="PaymentData"/>
        /// </summary>
        public PaymentData PaymentData
        {
            get
            {
                return paymentData;
            }
        }

        /// <summary>
        /// Any extra data not fitting the standard types.
        /// </summary>
        public IEnumerable<object> ExtraData
        {
            get
            {
                return extraData;
            }
        }
    }
}
