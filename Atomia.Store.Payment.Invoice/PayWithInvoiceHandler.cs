using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.Payment.Invoice
{
    public sealed class PayWithInvoiceHandler : PaymentDataHandler
    {
        public override string Id
        {
            get { return "PayWithInvoice"; }
        }

        public override PaymentMethodEnum PaymentMethodType
        {
            get { return PaymentMethodEnum.PayByInvoice; }
        }

        public override PublicOrder AmendOrder(PublicOrder order, PaymentData paymentData)
        {
            var form = paymentData.PaymentForm as PayWithInvoiceForm;
            
            Add(order, new PublicOrderCustomData { Name = "PayByInvoice", Value = "true" });

            if (form.SelectedInvoiceType == "post")
            {
                Add(order, new PublicOrderCustomData { Name = "SendInvoiceByPost", Value = "true" });
            }

            return order;
        }
    }
}
