using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;

namespace Atomia.Store.Payment.Invoice
{
    public class InvoiceByEmailGuiPlugin : PaymentMethodGuiPlugin
    {
        private readonly IResourceProvider resourceProvider;

        public InvoiceByEmailGuiPlugin(IResourceProvider resourceProvider)
        {
            if (resourceProvider == null)
            {
                throw new ArgumentNullException("resourceProvider");
            }

            this.resourceProvider = resourceProvider;
        }

        public override string Id
        {
            get { return "InvoiceByEmail"; }
        }

        public override string Name
        {
            get 
            { 
                return resourceProvider.GetResource("InvoiceByEmailName"); 
            }
        }

        public override string Description
        {
            get
            {
                return resourceProvider.GetResource("InvoiceByEmailDescription"); 
            }
        }
    }
}
