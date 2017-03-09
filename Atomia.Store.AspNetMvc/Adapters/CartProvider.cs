using Atomia.Store.Core;
using System.Web;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Adapters
{
    /// <summary>
    /// Session backed <see cref="Atomia.Store.Core.ICartProvider"/>
    /// </summary>
    public sealed class CartProvider : ICartProvider
    {
        private readonly ICartPricingService cartPricingProvider = DependencyResolver.Current.GetService<ICartPricingService>();
        private readonly IVatDisplayPreferenceProvider vatDisplayPreferenceProvider = DependencyResolver.Current.GetService<IVatDisplayPreferenceProvider>();

        /// <inheritdoc/>
        public Cart GetCart() 
        {
            var cart = HttpContext.Current.Session["Cart"] as Cart;

            if (cart == null)
            {
                cart = new Cart(this, cartPricingProvider, vatDisplayPreferenceProvider);
            }

            return cart;
        }

        /// <inheritdoc/>
        public void SaveCart(Cart cart)
        {
            HttpContext.Current.Session["Cart"] = cart;
        }

        /// <inheritdoc/>
        public void ClearCart()
        {
            HttpContext.Current.Session["Cart"] = null;
        }
    }
}
