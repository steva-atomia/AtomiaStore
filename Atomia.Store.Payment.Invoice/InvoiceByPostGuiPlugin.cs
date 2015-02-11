using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;

namespace Atomia.Store.Payment.Invoice
{
    public class InvoiceByPostGuiPlugin : PaymentMethodGuiPlugin
    {
        private readonly IResourceProvider resourceProvider;

        public InvoiceByPostGuiPlugin(IResourceProvider resourceProvider)
        {
            if (resourceProvider == null)
            {
                throw new ArgumentNullException("resourceProvider");
            }

            this.resourceProvider = resourceProvider;
        }

        public override string Id
        {
            get { return "InvoiceByPost"; }
        }

        public override string Name
        {
            get 
            { 
                return resourceProvider.GetResource("InvoiceByPostName"); 
            }
        }

        public override string Description
        {
            // TODO: Get price from actual item
            get
            {
                return resourceProvider.GetResource("InvoiceByPostDescription"); 
            }
        }
    }
}
