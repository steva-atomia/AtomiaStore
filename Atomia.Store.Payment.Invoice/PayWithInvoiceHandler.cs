using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi.Handlers;
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

        public override PublicOrder AmendOrder(PublicOrder order, PaymentData paymentMethodData)
        {
            var data = paymentMethodData as PayWithInvoiceGuiPlugin;
            
            Add(order, new PublicOrderCustomData { Name = "PayByInvoice", Value = "true" });

            if (data.SelectedInvoiceType == "post")
            {
                Add(order, new PublicOrderCustomData { Name = "SendInvoiceByPost", Value = "true" });
            }

            return order;
        }
    }
}
