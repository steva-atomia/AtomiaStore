
namespace Atomia.Store.Core
{
    /// <summary>
    /// Each <see cref="Product"/> can have multiple pricing variants
    /// </summary>
    public sealed class PricingVariant
    {
        /// <summary>
        /// The <see cref="RenewalPeriod"/>, if any for the product.
        /// </summary>
        /// <remarks>Set to null means no renewal period.</remarks>
        public RenewalPeriod RenewalPeriod { get; set; }

        /// <summary>
        /// The price
        /// </summary>
        public decimal Price { get; set; }
    }
}
