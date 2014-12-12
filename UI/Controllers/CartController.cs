using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atomia.Store.Core;
using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public sealed class CartController : Controller
    {
        private readonly Cart cart;
        private readonly IItemDisplayProvider displayProvider;
        private readonly ICurrencyProvider currencyProvider;

        public CartController(ICartProvider cartProvider, IItemDisplayProvider displayProvider, ICurrencyProvider currencyProvider)
        {
            this.cart = cartProvider.GetCart();
            this.displayProvider = displayProvider;
            this.currencyProvider = currencyProvider;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(cart);
        }

        [ChildActionOnly]
        public ActionResult Partial()
        {
            return PartialView(cart);
        }

        [HttpPost]
        public JsonResult AddItem(CartItemInput inputItem)
        {
            if (ModelState.IsValid)
            {
                var cartItem = inputItem.ToCartItem(displayProvider, currencyProvider);
                cart.AddItem(cartItem);

                return JsonEnvelope.Success(new { Cart = cart });
            }

            return JsonEnvelope.Fail(ModelState);
        }

        [HttpPost]
        public JsonResult RemoveItem(int itemId)
        {
            if (ModelState.IsValid)
            {
                cart.RemoveItem(itemId);
                return JsonEnvelope.Success(new { Cart = cart });
            }

            return JsonEnvelope.Fail(ModelState);
        }

        [HttpPost]
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
