using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.PublicBillingApi.Handlers
{
    public abstract class PaymentDataHandler
    {
        public abstract string Id { get; }

        public abstract PaymentMethodEnum PaymentMethodType { get; }

        /// <summary>
        /// Amend order with payment method specific attributes.
        /// </summary>
        /// <param name="order">The order to amend.</param>
        public virtual PublicOrder AmendOrder(PublicOrder order, PaymentData paymentData)
        {
            return order;
        }

        /// <summary>
        /// Amend payment transaction with payment method specific attributes.
        /// Typical is to set ReturnUrl on transaction and add CancelUrl to attributes.
        /// </summary>
        /// <param name="transaction">The transaction to amend</param>
        public virtual PublicPaymentTransaction AmendPaymentTransaction(PublicPaymentTransaction paymentTransaction, PaymentData paymentData)
        {
            return paymentTransaction;
        }

        protected PublicPaymentTransaction SetCancelUrl(PublicPaymentTransaction paymentTransaction, string cancelUrl)
        {
            var cancelAttribute = paymentTransaction.Attributes.FirstOrDefault(item => item.Name == "CancelUrl");

            if (cancelAttribute != null)
            {
                cancelAttribute.Value = cancelUrl;
            }
            else
            {
                var transactionAttributes = new List<AttributeData>(paymentTransaction.Attributes);
                transactionAttributes.Add(new AttributeData
                {
                    Name = "CancelUrl",
                    Value = cancelUrl
                });

                paymentTransaction.Attributes = transactionAttributes.ToArray();
            }

            return paymentTransaction;
        }

        protected void Add(PublicOrder order, PublicOrderCustomData customData)
        {
            var customAttributes = new List<PublicOrderCustomData>(order.CustomData);

            customAttributes.Add(customData);

            order.CustomData = customAttributes.ToArray();
        }
    }
}
