using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atomia.Store.Core;
using Atomia.Store.UI.Infrastructure;

namespace Atomia.Store.UI.Controllers
{
    public sealed class CartController : Controller
    {
        private readonly Cart cart;

        public CartController(ICartRepository cartRepository)
        {
            this.cart = cartRepository.GetCart();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Partial()
        {
            return PartialView();
        }

        public JsonResult AddItem(CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                cart.AddItem(cartItem);
                return JsonEnvelope.Success(new { Cart = cart });
            }

            return JsonEnvelope.Fail(ModelState);
        }

        public JsonResult RemoveItem(int itemId)
        {
            if (ModelState.IsValid)
            {
                cart.RemoveItem(itemId);
                return JsonEnvelope.Success(new { Cart = cart });
            }

            return JsonEnvelope.Fail(ModelState);
        }

        public JsonResult ChangeQuantity(int itemId, decimal newQuantity)
        {
            if (ModelState.IsValid)
            {
                cart.ChangeQuantity(itemId, newQuantity);
                return JsonEnvelope.Success(new { Cart = cart });
            }

            return JsonEnvelope.Fail(ModelState);
        }
    }
}
