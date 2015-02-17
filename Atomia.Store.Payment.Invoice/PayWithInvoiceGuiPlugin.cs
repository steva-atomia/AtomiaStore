using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;


namespace Atomia.Store.Payment.Invoice
{
    public class PayWithInvoiceGuiPlugin : PaymentMethodGuiPlugin
    {
        private readonly IResourceProvider resourceProvider;

        public PayWithInvoiceGuiPlugin(IResourceProvider resourceProvider)
        {
            if (resourceProvider == null)
            {
                throw new ArgumentNullException("resourceProvider");
            }

            this.resourceProvider = resourceProvider;
        }

        public override string Id
        {
            get { return "PayWithInvoice"; }
        }

        public override string Name
        {
            get 
            {
                return resourceProvider.GetResource("PayWithInvoiceName"); 
            }
        }

        public override string Description
        {
            get
            {
                return resourceProvider.GetResource("PayWithInvoiceDescription"); 
            }
        }

        public override PaymentMethodForm Form
        {
            get
            {
                return new PayWithInvoiceForm();
            }
        }

        public string SelectedInvoiceType
        {
            get
            {
                var form = (PayWithInvoiceForm) Form;

                return form.SelectedInvoiceType;
            }
        }
    }
}
