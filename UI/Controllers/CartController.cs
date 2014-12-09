using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atomia.Store.Core.Cart;
using Atomia.Store.Core.Products;
using Atomia.Store.UI.Infrastructure;

namespace Atomia.Store.UI.Controllers
{
    public sealed class CartController : Controller
    {
        private readonly Cart cart;
        private readonly IProductNameProvider productNameProvider;

        public CartController(ICartRepository cartService, IProductNameProvider productNameProvider)
        {
            this.cart = cartService.GetCart();
            this.productNameProvider = productNameProvider;
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

        public JsonResult AddItem(string articleNumber, RenewalPeriod renewalPeriod, decimal quantity)
        {
            cart.AddItem(new CartItem(productNameProvider, articleNumber)
            {
                RenewalPeriod = renewalPeriod,
                Quantity = quantity
            });

            return JsonEnvelope.Success(new { Cart = cart });
        }

        public JsonResult RemoveItem(int itemId)
        {
            cart.RemoveItem(itemId);

            return JsonEnvelope.Success(new { Cart = cart });
        }
    }
}
