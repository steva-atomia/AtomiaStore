
namespace Atomia.Store.Core
{
    public interface ICartProvider
    {
        Cart GetCart();

        void SaveCart(Cart cart);
    }
}
