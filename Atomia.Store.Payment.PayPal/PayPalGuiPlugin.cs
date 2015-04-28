using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;
using System;

namespace Atomia.Store.Payment.PayPal
{
    public sealed class PayPalGuiPlugin : PaymentMethodGuiPlugin
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

        public override bool SupportsPaymentProfile
        {
            get { return true; }
        }
    }
}
