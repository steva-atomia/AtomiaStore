using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.PublicBillingApi.Ports;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.Payment.Invoice
{
    public class InvoiceByEmailHandler : PaymentMethodHandler
    {
        public override string Id
        {
            get { return "InvoiceByEmail"; }
        }

        public override PaymentMethodEnum PaymentMethodType
        {
            get { return PaymentMethodEnum.PayByInvoice; }
        }

        public override void AmendOrder(PublicOrder order, List<PublicOrderCustomData> customData)
        {
            customData.Add(new PublicOrderCustomData { Name = "PayByInvoice", Value = "true" });
        }
    }
}
