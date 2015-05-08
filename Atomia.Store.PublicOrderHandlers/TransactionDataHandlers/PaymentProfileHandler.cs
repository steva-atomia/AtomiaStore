using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System.Collections.Generic;

namespace Atomia.Store.PublicOrderHandlers.TransactionDataHandlers
{
    /// <summary>
    /// Handler to amend payment transaction with payment profile attributes
    /// </summary>
    public sealed class PaymentProfileHandler : TransactionDataHandler
    {
        /// <summary>
        /// Amend payment transaction with payment profile attributes
        /// </summary>
        public override PublicPaymentTransaction AmendPaymentTransaction(PublicPaymentTransaction paymentTransaction, PublicOrderContext orderContext)
        {
            var paymentData = orderContext.PaymentData;

            if (paymentData.SaveCcInfo)
            {
                var attributes = new List<AttributeData>(paymentTransaction.Attributes);

                attributes.Add(new AttributeData { Name = "cc_saveccinfo", Value = "true" });

                if (paymentData.AutoPay)
                {
                    attributes.Add(new AttributeData { Name = "cc_autopay", Value = "true"  });
                }

                paymentTransaction.Attributes = attributes.ToArray();
            }

            return paymentTransaction;
        }
    }
}
