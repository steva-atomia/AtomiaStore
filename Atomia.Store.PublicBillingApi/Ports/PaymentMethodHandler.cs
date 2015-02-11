using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.PublicBillingApi.Ports
{
    public abstract class PaymentMethodHandler
    {
        public abstract string Id { get; }

        public abstract PaymentMethodEnum PaymentMethodType { get; }

        /// <summary>
        /// Amend order with payment method specific attributes.
        /// </summary>
        /// <param name="order">The order to amend.</param>
        public virtual void AmendOrder(PublicOrder order, List<PublicOrderCustomData> customData)
        {
        }

        /// <summary>
        /// Amend payment transaction with payment method specific attributes.
        /// Typical is to set ReturnUrl on transaction and add CancelUrl to attributes.
        /// </summary>
        /// <param name="transaction">The transaction to amend</param>
        public virtual void AmendTransaction(PublicPaymentTransaction transaction, List<AttributeData> attributes)
        {
        }
    }
}
