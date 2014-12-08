using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atomia.Store.Core.Cart;
using Atomia.Store.UI.Infrastructure;

namespace Atomia.Store.UI.Controllers
{
    public sealed class CartController : Controller
    {
        private readonly Cart cart;

        public CartController(ICartRepository cartService)
        {
            this.cart = cartService.GetCart();
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
                cart.Add(cartItem);

                return JsonEnvelope.Success(cart);
            }

            return JsonEnvelope.Fail(null);
        }

        public JsonResult RemoveItem()
        {
            return new JsonResult();
        }
    }
}
