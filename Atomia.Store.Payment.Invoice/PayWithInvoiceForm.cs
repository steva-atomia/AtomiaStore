using Atomia.Store.AspNetMvc.Ports;
using Atomia.Web.Plugin.Validation.ValidationAttributes;

namespace Atomia.Store.Payment.Invoice
{
    public class PayWithInvoiceForm : PaymentMethodForm
    {
        public override string Id
        {
            get { return "PayWithInvoice"; }
        }

        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        public string SelectedInvoiceType { get; set; }
    }
}
