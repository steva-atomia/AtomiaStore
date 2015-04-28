using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;
using System;

namespace Atomia.Store.Payment.CCPayment
{
    public sealed class CCPaymentGuiPlugin : PaymentMethodGuiPlugin
    {
        private readonly IResourceProvider resourceProvider;
        private CCPaymentForm form;

        public CCPaymentGuiPlugin(IResourceProvider resourceProvider)
        {
            if (resourceProvider == null)
            {
                throw new ArgumentNullException("resourceProvider");
            }

            this.resourceProvider = resourceProvider;
            this.form = new CCPaymentForm();
        }

        public override string Id
        {
            get { return "CCPayment"; }
        }

        public override string Name
        {
            get
            {
                return resourceProvider.GetResource("CCPaymentName");
            }
        }

        public override bool SupportsPaymentProfile
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
                form = value as CCPaymentForm;
            }
        }
    }
}
