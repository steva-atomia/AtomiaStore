using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;
using System;


namespace Atomia.Store.Payment.AdyenHpp
{
    public sealed class AdyenHppGuiPlugin : PaymentMethodGuiPlugin
    {
        private readonly IResourceProvider resourceProvider;

        public AdyenHppGuiPlugin(IResourceProvider resourceProvider)
        {
            if (resourceProvider == null)
            {
                throw new ArgumentNullException("resourceProvider");
            }

            this.resourceProvider = resourceProvider;
        }

        public override string Id
        {
            get { return "AdyenHpp"; }
        }

        public override string Name
        {
            get 
            {
                return resourceProvider.GetResource("AdyenHppName");
            }
        }

        public override string Description
        {
            get
            {
                return resourceProvider.GetResource("AdyenHppDescription");
            }
        }
    }
}
