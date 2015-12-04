using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Controllers
{
    /// <summary>
    /// JSON cart API.
    /// </summary>
    public sealed class CartController : Controller
    {
        private readonly ICartProvider cartProvider = DependencyResolver.Current.GetService<ICartProvider>();
        private readonly IDomainsProvider domainsProvider = DependencyResolver.Current.GetService<IDomainsProvider>();

        /// <summary>
        /// Get the current cart and domain categories.
        /// </summary>
        public JsonResult GetCart()
        {
            var cart = cartProvider.GetCart();

            return JsonEnvelope.Success(new
                {
                    Cart = new CartModel(cart),
                    DomainCategories = domainsProvider.GetDomainCategories()
                });
        }

        /// <summary>
        /// Update, recalculate and save current cart based on POST:ed data.
        /// </summary>
        [HttpPost]
        public JsonResult RecalculateCart(CartUpdateModel updatedCart)
        {
            if (ModelState.IsValid)
            {
                var cart = cartProvider.GetCart();

                cart.Clear();
                foreach (var attr in updatedCart.CustomAttributes)
                {
                    cart.SetCustomAttribute(attr.Name, attr.Value);
                }
                cart.UpdateCart(updatedCart.CartItems.Select(ci => ci.CartItem), updatedCart.CampaignCode);

                return JsonEnvelope.Success(new {
                    Cart = new CartModel(cart)
                });
            }

            return JsonEnvelope.Fail(ModelState);
        }
    }
}
