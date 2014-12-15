using Atomia.Store.Core;
using Atomia.Store.AspNetMvc.Models;
using System.Web;

namespace Atomia.Store.AspNetMvc.Services
{
    public class CartProvider : ICartProvider
    {
        private readonly ICartPricingService cartPricingProvider;

        public CartProvider(ICartPricingService cartPricingProvider)
        {
            this.cartPricingProvider = cartPricingProvider;
        }
        
        public Cart GetCart() 
        {
            var cart = HttpContext.Current.Session["Cart"] as Cart;

            if (cart == null)
            {
                cart = new Cart(this, cartPricingProvider);
            }

            return cart;
        }

        public void SaveCart(Cart cart)
        {
            HttpContext.Current.Session["Cart"] = cart;
        }
    }
}
