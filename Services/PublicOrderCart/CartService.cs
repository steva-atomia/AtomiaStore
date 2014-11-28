using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Atomia.Store.Core.Cart;
using Atomia.Store.Core.Products;
using Atomia.Store.Core.Services;

namespace Atomia.Store.Services.PublicOrderCart
{
    public class CartService : ICartService
    {
        private readonly IResellerService resellerService;
        private readonly IProductService productService;

        public CartService(IResellerService resellerService, IProductService productService)
        {
            this.resellerService = resellerService;
            this.productService = productService;
        }
        
        public ICart GetCart() 
        {
            var cart = HttpContext.Current.Session["Cart"] as Cart;

            if (cart == null)
            {
                cart = new Cart(resellerService, productService, this);
            }

            return cart;
        }

        public void SaveCart(ICart cart)
        {
            HttpContext.Current.Session["Cart"] = cart;
        }
    }
}
