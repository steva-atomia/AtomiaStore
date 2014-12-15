using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public sealed class CartController : Controller
    {
        private readonly Cart cart;

        public CartController(ICartProvider cartProvider)
        {
            this.cart = cartProvider.GetCart();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new CartModel(cart));
        }

        [ChildActionOnly]
        public ActionResult Partial()
        {
            return PartialView(new CartModel(cart));
        }

        [HttpPost]
        public JsonResult AddItem(CartItemModel item)
        {
            if (ModelState.IsValid)
            {
                cart.AddItem(item.CartItem);

                return JsonEnvelope.Success(new CartModel(cart));
            }

            return JsonEnvelope.Fail(ModelState);
        }

        [HttpPost]
        public JsonResult RemoveItem(int itemId)
        {
            if (ModelState.IsValid)
            {
                cart.RemoveItem(itemId);

                return JsonEnvelope.Success(new CartModel(cart));
            }

            return JsonEnvelope.Fail(ModelState);
        }

        [HttpPost]
        public JsonResult ChangeQuantity(CartItemQuantityModel cartItemQuantity)
        {
            if (ModelState.IsValid)
            {
                cart.ChangeQuantity(cartItemQuantity.Id, cartItemQuantity.Quantity);

                return JsonEnvelope.Success(new CartModel(cart));
            }

            return JsonEnvelope.Fail(ModelState);
        }
    }
}
