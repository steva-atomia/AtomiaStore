using Atomia.Store.Core;
using System.Web;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Adapters
{
    public class CartProvider : ICartProvider
    {
        private readonly ICartPricingService cartPricingProvider = DependencyResolver.Current.GetService<ICartPricingService>();

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


        public void ClearCart()
        {
            HttpContext.Current.Session["Cart"] = null;
        }
    }
}
