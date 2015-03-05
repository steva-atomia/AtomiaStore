using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;
using System;


namespace Atomia.Store.Payment.Invoice
{
    public class PayWithInvoiceGuiPlugin : PaymentMethodGuiPlugin
    {
        private readonly IResourceProvider resourceProvider;
        private PayWithInvoiceForm form;

        public PayWithInvoiceGuiPlugin(IResourceProvider resourceProvider)
        {
            if (resourceProvider == null)
            {
                throw new ArgumentNullException("resourceProvider");
            }

            this.resourceProvider = resourceProvider;
            this.form = new PayWithInvoiceForm();
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

        public override bool HasForm
        {
            get { return true; }
        }

        public override PaymentMethodForm Form
        {
            get
            {
                return form;
            }
            set
            {
                form = value as PayWithInvoiceForm;
            }
        }

        public string SelectedInvoiceType
        {
            get
            {
                if (form != null)
                {
                    return form.SelectedInvoiceType;
                }

                return String.Empty;
            }
        }
    }
}
