using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;

namespace Atomia.Store.Payment.PayPal
{
    public class PayPalGuiPlugin : PaymentMethodGuiPlugin
    {
        private readonly IResourceProvider resourceProvider;

        public PayPalGuiPlugin(IResourceProvider resourceProvider)
        {
            if (resourceProvider == null)
            {
                throw new ArgumentNullException("resourceProvider");
            }

            this.resourceProvider = resourceProvider;
        }

        public override string Id
        {
            get { return "PayPal"; }
        }

        public override string Name
        {
            get 
            {
                return resourceProvider.GetResource("PayPalName");
            }
        }

        public override string Description
        {
            get
            {
                return resourceProvider.GetResource("PayPalDescription");
            }
        }
    }
}
