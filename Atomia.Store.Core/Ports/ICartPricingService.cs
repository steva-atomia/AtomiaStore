
namespace Atomia.Store.Core
{
    /// <summary>
    /// Service to calculate prices for items and totals of a <see cref="Cart"/>
    /// </summary>
    public interface ICartPricingService
    {
        /// <summary>
        /// Calculate the prices and totals of <see cref="Cart"/> and contained items.
        /// </summary>
        /// <param name="cart">The <see cref="Cart"/> to calculate prices and totals for</param>
        /// <returns>The <see cref="Cart"/> with calculated prices and totals set.</returns>
        Cart CalculatePricing(Cart cart);
    }
}
