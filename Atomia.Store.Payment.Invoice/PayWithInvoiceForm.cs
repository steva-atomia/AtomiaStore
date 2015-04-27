using Atomia.Store.AspNetMvc.Ports;
using Atomia.Web.Plugin.Validation.ValidationAttributes;

namespace Atomia.Store.Payment.Invoice
{
    public sealed class PayWithInvoiceForm : PaymentMethodForm
    {
        public override string Id
        {
            get { return "PayWithInvoice"; }
        }

        [AtomiaRequired("Common,ErrorEmptyField")]
        public string SelectedInvoiceType { get; set; }
    }
}
