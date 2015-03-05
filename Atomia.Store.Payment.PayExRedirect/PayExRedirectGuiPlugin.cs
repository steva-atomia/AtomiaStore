using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;
using System;

namespace Atomia.Store.Payment.PayExRedirect
{
    public sealed class PayExRedirectGuiPlugin : PaymentMethodGuiPlugin
    {
        private readonly IResourceProvider resourceProvider;

        public PayExRedirectGuiPlugin(IResourceProvider resourceProvider)
        {
            if (resourceProvider == null)
            {
                throw new ArgumentNullException("resourceProvider");
            }

            this.resourceProvider = resourceProvider;
        }

        public override string Id
        {
            get { return "PayExRedirect"; }
        }

        public override string Name
        {
            get 
            {
                return resourceProvider.GetResource("PayExRedirectName");
            }
        }

        public override string Description
        {
            get
            {
                return resourceProvider.GetResource("PayExRedirectDescription");
            }
        }
    }
}
