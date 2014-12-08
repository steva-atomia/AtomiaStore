using Atomia.Store.Core.Cart;
using System.Web;

namespace Atomia.Store.UI.Storage
{
    public class CartRepository : ICartRepository
    {
        private readonly ICartPricingProvider cartPricingProvider;

        public CartRepository(ICartPricingProvider cartPricingProvider)
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
