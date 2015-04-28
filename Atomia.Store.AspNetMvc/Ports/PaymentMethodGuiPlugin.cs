using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Ports
{
    /// <summary>
    /// Abstract representation of payment method that can be rendered by GUI.
    /// </summary>
    public abstract class PaymentMethodGuiPlugin
    {
        /// <summary>
        /// <see cref="PaymentMethod"/> identifier
        /// </summary>
        public abstract string Id { get; }

        /// <summary>
        /// Localized name of the payment method
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Localized description / instructions for the payment method.
        /// </summary>
        public virtual string Description
        {
            get { return ""; }
        }

        /// <summary>
        /// Whether payment method supports saving payment info and autopay or not.
        /// </summary>
        public virtual bool SupportsPaymentProfile
        {
            get { return false; }
        }

        /// <summary>
        /// A <see cref="PaymentMethodForm"/> implementation for collecting any needed extra payment method data from the customer.
        /// </summary>
        public virtual PaymentMethodForm Form { get; set; }
    }
}
