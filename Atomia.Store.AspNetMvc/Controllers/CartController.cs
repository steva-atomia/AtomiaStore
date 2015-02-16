using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System.Web.Mvc;
using System.Linq;
using System;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public sealed class CartController : Controller
    {
        private readonly Cart cart;
        private readonly IDomainsProvider domainsProvider;

        public CartController()
        {
            var cartProvider = DependencyResolver.Current.GetService<ICartProvider>();
            
            this.domainsProvider = DependencyResolver.Current.GetService<IDomainsProvider>();
            this.cart = cartProvider.GetCart();
        }

        public JsonResult GetCart()
        {
            return JsonEnvelope.Success(new
                {
                    Cart = new CartDataModel(cart),
                    DomainCategories = domainsProvider.GetDomainCategories()
                });
        }

        [HttpPost]
        public JsonResult RecalculateCart(CartUpdateModel updatedCart)
        {
            if (ModelState.IsValid)
            {
                cart.Clear();
                cart.UpdateCart(updatedCart.CartItems.Select(ci => ci.CartItem), updatedCart.CampaignCode);
                
                return JsonEnvelope.Success(new {
                    Cart = new CartDataModel(cart)
                });
            }

            return JsonEnvelope.Fail(ModelState);
        }
    }
}
