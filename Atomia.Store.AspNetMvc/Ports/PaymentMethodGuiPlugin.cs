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
        /// If the payment method has an accompanying form. Defaults to false.
        /// </summary>
        /// <remarks>If this is true, the Form property should return a <see cref="PaymenMethodForm"/> implementation</remarks>
        public virtual bool HasForm
        {
            get { return false; }
        }

        /// <summary>
        /// A <see cref="PaymentMethodForm"/> implementation for collecting any needed extra payment method data from the customer.
        /// </summary>
        public virtual PaymentMethodForm Form { get; set; }
    }
}
