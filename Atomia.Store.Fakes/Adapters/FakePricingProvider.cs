using Atomia.Store.Core;
using System.Linq;
using System.Collections.Generic;

namespace Atomia.Store.Fakes.Adapters
{
    public sealed class FakePricingProvider : ICartPricingService
    {
        public Cart CalculatePricing(Cart cart)
        {
            foreach(var cartItem in cart.CartItems)
            {
                cartItem.SetPricing(10, 2, new List<Tax>());
            }

            cart.SetPricing(cart.CartItems.Sum(ci => ci.Price), cart.CartItems.Sum(ci => ci.Total), new List<Tax>());

            return cart;
        }
    }
}
