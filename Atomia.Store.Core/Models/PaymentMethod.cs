
namespace Atomia.Store.Core
{
    /// <summary>
    /// Payment method, like invoice or PayPal.
    /// </summary>
    public sealed class PaymentMethod
    {
        /// <summary>
        /// Unique id for each payment method
        /// </summary>
        public string Id { get; set; }
    }
}
