using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Ports
{
    public abstract class PaymentMethodGuiPlugin : PaymentData
    {
        public abstract string Name { get; }

        public virtual string Description
        {
            get
            {
                return "";
            }
        }

        public virtual bool HasForm
        {
            get { return false; }
        }

        public virtual PaymentMethodForm Form { get; set; }
    }
}
