
namespace Atomia.Store.Core
{
    /// <summary>
    /// Abstract class for providing URLs that are of use when implementing payment method related functionality.
    /// </summary>
    public abstract class PaymentUrlProvider
    {
        /// <summary>
        /// URL to go to after successful payment.
        /// </summary>
        public abstract string SuccessUrl { get; set; }

        /// <summary>
        /// URL to go to after failed payment.
        /// </summary>
        public abstract string FailureUrl { get; }

        /// <summary>
        /// URL to go to after cancelled payment.
        /// </summary>
        public abstract string CancelUrl { get; }

        /// <summary>
        /// URL for action that implements method where parameters correspond to the standard parameters sent by Atomia payment HTTP handlers.
        /// </summary>
        public abstract string DefaultPaymentUrl { get; }

        /// <summary>
        /// Get URL including hostname from a path.
        /// </summary>
        /// <param name="path">The path to qualify</param>
        /// <returns>The URL including hostname</returns>
        public abstract string QualifiedUrl(string path);
    }
}
