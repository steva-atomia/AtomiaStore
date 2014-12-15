using Atomia.Store.Core;
using System.Web;

namespace Atomia.Store.AspNetMvc.Services
{
    public class CartProvider : ICartProvider
    {
        private readonly ICartPricingService cartPricingProvider;
        private readonly ICartItemProvider cartItemProvider;

        public CartProvider(ICartPricingService cartPricingProvider, ICartItemProvider cartItemProvider)
        {
            this.cartPricingProvider = cartPricingProvider;
            this.cartItemProvider = cartItemProvider;
        }
        
        public Cart GetCart() 
        {
            var cart = HttpContext.Current.Session["Cart"] as Cart;

            if (cart == null)
            {
                cart = new Cart(this, cartPricingProvider, cartItemProvider);
            }

            return cart;
        }

        public void SaveCart(Cart cart)
        {
            HttpContext.Current.Session["Cart"] = cart;
        }
    }
}
