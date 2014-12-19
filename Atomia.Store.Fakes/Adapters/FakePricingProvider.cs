using Atomia.Store.Core;
using System.Linq;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakePricingProvider : ICartPricingService
    {
        public Cart CalculatePricing(Cart cart)
        {
            foreach(var cartItem in cart.CartItems)
            {
                cartItem.SetPricing(10, 0, 2);
            }

            cart.SetPricing(cart.CartItems.Sum(ci => ci.Price), cart.CartItems.Sum(ci => ci.TaxAmount), cart.CartItems.Sum(ci => ci.Total));

            return cart;
        }
    }
}
