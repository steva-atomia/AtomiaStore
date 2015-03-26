
namespace Atomia.Store.Core
{
    /// <summary>
    /// Interface for persisting the current <see cref="Cart"/>
    /// </summary>
    public interface ICartProvider
    {
        /// <summary>
        /// Get the user's current <see cref="Cart"/>
        /// </summary>
        Cart GetCart();

        /// <summary>
        /// Save the user's current <see cref="Cart"/>
        /// </summary>
        void SaveCart(Cart cart);

        /// <summary>
        /// Throw away the user's current <see cref="Cart"/>
        /// </summary>
        void ClearCart();
    }
}
