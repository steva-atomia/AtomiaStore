using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi.Ports;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.Payment.Invoice
{
    public class PayWithInvoiceHandler : PaymentDataHandler
    {
        public override string Id
        {
            get { return "PayWithInvoice"; }
        }

        public override PaymentMethodEnum PaymentMethodType
        {
            get { return PaymentMethodEnum.PayByInvoice; }
        }

        public override void AmendOrder(PaymentData paymentMethodData, PublicOrder order, List<PublicOrderCustomData> customData)
        {
            var data = paymentMethodData as PayWithInvoiceGuiPlugin;
            
            customData.Add(new PublicOrderCustomData { Name = "PayByInvoice", Value = "true" });

            if (data.SelectedInvoiceType == "post")
            {
                customData.Add(new PublicOrderCustomData { Name = "SendInvoiceByPost", Value = "true" });
            }
        }
    }
}
