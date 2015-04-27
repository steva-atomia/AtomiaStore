using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Ports
{
    /// <summary>
    /// Abstract representation of payment method that can be rendered by GUI.
    /// </summary>
    public abstract class PaymentMethodGuiPlugin : PaymentData
    {
        /// <summary>
        /// Localized name of the payment method
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Localized description / instructions for the payment method.
        /// </summary>
        public virtual string Description
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// A <see cref="PaymentMethodForm"/> implementation for collecting any needed extra payment method data from the customer.
        /// </summary>
        public virtual PaymentMethodForm Form { get; set; }
    }
}
