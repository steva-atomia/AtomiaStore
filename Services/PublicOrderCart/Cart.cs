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
    public class Cart : ICart
    {
        private readonly IResellerService resellerService;
        private readonly IProductService productService;
        private readonly ICartService cartService;

        private HashSet<string> campaignCodes = new HashSet<string>();
        private List<CartItem> cartItems = new List<CartItem>();
        private bool priceUpdated = false;

        public Cart(IResellerService resellerService, IProductService productService, ICartService cartService)
        {
            this.resellerService = resellerService;
            this.productService = productService;
            this.cartService = cartService;
        }

        public IEnumerable<CartItem> CartItems
        {
            get
            {
                return this.cartItems;
            }
        }

        public IEnumerable<string> CampaignCodes
        {
            get
            {
                return this.campaignCodes;
            }
        }

        public Pricing Pricing
        {
            get
            {
                if (!priceUpdated)
                {

                }
                
                // TODO: Build up a PublicOrder from all cartItems and campaignCodes and call CalculateOrder in PublicOrderApi.
                // Advanced: only recheck in orderApi if cartHasChanged
                var pricing = new Pricing(1m, 1m, 1m, 1m);

                this.priceUpdated = true;

                return pricing;
            }
        }

        public void Add(CartItem cartItem)
        {
            // TODO: add pricing to the cartItem.

            this.cartItems.Add(cartItem);

            cartService.SaveCart(this);
        }

        public void Remove(CartItem cartItem)
        {
            this.cartItems.Remove(cartItem);

            cartService.SaveCart(this);
        }

        public void Add(string campaignCode)
        {
            // TODO: Validate campaignCode?

            this.campaignCodes.Add(campaignCode);

            cartService.SaveCart(this);
        }

        public void Remove(string campaignCode)
        {
            this.campaignCodes.Remove(campaignCode);

            cartService.SaveCart(this);
        }

        public void Clear()
        {
            this.cartItems.Clear();
            this.campaignCodes.Clear();

            cartService.SaveCart(this);
        }
    }
}
