
namespace Atomia.Store.AspNetMvc.Ports
{
    /// <summary>
    /// Abstract payment method form that can optionally be implemented and added to <see cref="PaymentMethodGuiPlugin">PaymentMethodGuiPlugins</see>
    /// It is expected that implementations add properties for the form fields.
    /// </summary>
    public abstract class PaymentMethodForm
    {
        /// <summary>
        /// The id of the payment method this form belongs to.
        /// </summary>
        public abstract string Id { get; }

        /// <summary>
        /// The name of a partial view that can render this form. Default value based on payment method id.
        /// </summary>
        public virtual string PartialViewName { get { return "_" + this.Id; } }
    }
}
