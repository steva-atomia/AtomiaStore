using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;
using System;

namespace Atomia.Store.Payment.WorldPayXml
{
    public class WorldPayXmlGuiPlugin : PaymentMethodGuiPlugin
    {
        private readonly IResourceProvider resourceProvider;

        public WorldPayXmlGuiPlugin(IResourceProvider resourceProvider)
        {
            if (resourceProvider == null)
            {
                throw new ArgumentNullException("resourceProvider");
            }

            this.resourceProvider = resourceProvider;
        }

        public override string Id
        {
            get { return "WorldPayXmlRedirect"; }
        }

        public override string Name
        {
            get 
            {
                return resourceProvider.GetResource("WorldPayXmlRedirectName");
            }
        }

        public override string Description
        {
            get
            {
                return resourceProvider.GetResource("WorldPayXmlRedirectDescription");
            }
        }
    }
}
