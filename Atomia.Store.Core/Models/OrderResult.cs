
namespace Atomia.Store.Core
{
    /// <summary>
    /// Result from placing an order
    /// </summary>
    public sealed class OrderResult
    {
        /// <summary>
        /// URL that should be redirected to, e.g. a payment gateway.
        /// </summary>
        public string RedirectUrl { get; set; }
    }
}
