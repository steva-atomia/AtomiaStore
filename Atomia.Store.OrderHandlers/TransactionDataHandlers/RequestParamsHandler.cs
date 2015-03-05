using Atomia.Billing.Core.Common.PaymentPlugins;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Atomia.Store.PublicOrderHandlers.TransactionDataHandlers
{
    public class RequestParamsHandler : TransactionDataHandler
    {
        public override PublicPaymentTransaction AmendPaymentTransaction(PublicPaymentTransaction paymentTransaction, PublicOrderContext orderContext)
        {
            var attributes = new List<AttributeData>(paymentTransaction.Attributes);
            var request = orderContext.ExtraData.OfType<HttpRequestBase>().FirstOrDefault();

            if (request != null)
            {
                foreach (var key in GuiPaymentPluginRequestHelper.RequestToCustomAttributes(request))
                {
                    attributes.Add(new AttributeData { Name = key.Key, Value = key.Value });
                }
            }

            paymentTransaction.Attributes = attributes.ToArray();

            return paymentTransaction;
        }
    }
}
