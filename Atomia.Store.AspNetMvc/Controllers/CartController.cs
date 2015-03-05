using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public sealed class CartController : Controller
    {
        private readonly ICartProvider cartProvider = DependencyResolver.Current.GetService<ICartProvider>();
        private readonly IDomainsProvider domainsProvider = DependencyResolver.Current.GetService<IDomainsProvider>();

        public JsonResult GetCart()
        {
            var cart = cartProvider.GetCart();

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
                var cart = cartProvider.GetCart();

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
