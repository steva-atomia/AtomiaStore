using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;
using System;

namespace Atomia.Store.Payment.DibsFlexwin
{
    public sealed class DibsFlexwinGuiPlugin : PaymentMethodGuiPlugin
    {
        private readonly IResourceProvider resourceProvider;

        public DibsFlexwinGuiPlugin(IResourceProvider resourceProvider)
        {
            if (resourceProvider == null)
            {
                throw new ArgumentNullException("resourceProvider");
            }

            this.resourceProvider = resourceProvider;
        }

        public override string Id
        {
            get { return "DibsFlexwin"; }
        }

        public override string Name
        {
            get 
            {
                return resourceProvider.GetResource("DibsFlexwinName");
            }
        }

        public override string Description
        {
            get
            {
                return resourceProvider.GetResource("DibsFlexwinDescription");
            }
        }
    }
}
