using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
