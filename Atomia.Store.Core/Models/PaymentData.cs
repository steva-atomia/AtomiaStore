
namespace Atomia.Store.Core
{
    /// <summary>
    /// Base class for payment data input
    /// </summary>
    public abstract class PaymentData
    {
        /// <summary>
        /// <see cref="PaymentMethod"/> identifier
        /// </summary>
        public abstract string Id { get; }
    }
}
