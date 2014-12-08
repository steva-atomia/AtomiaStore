using System.Collections.Generic;

namespace Atomia.Store.Core.Cart
{
    public class Cart
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartPricingProvider cartPricingProvider;
        
        private List<CartItem> cartItems = new List<CartItem>();
        private string campaignCode;

        public Cart(ICartRepository cartRepository, ICartPricingProvider cartPricingProvider)
        {
            this.cartRepository = cartRepository;
            this.cartPricingProvider = cartPricingProvider;
        }

        public IEnumerable<CartItem> CartItems
        {
            get
            {
                return this.cartItems;
            }
        }

        public string CampaignCode { get { return campaignCode; }}

        public decimal SubTotal { get; set; }

        public decimal Tax { get; set; }

        public decimal Total { get; set; }

        public void AddItem(CartItem cartItem)
        {
            this.cartItems.Add(cartItem);

            var updatedCart = cartPricingProvider.CalculatePricing(this);
            cartRepository.SaveCart(updatedCart);
        }

        public void RemoveItem(CartItem cartItem)
        {
            this.cartItems.Remove(cartItem);

            var updatedCart = cartPricingProvider.CalculatePricing(this);
            cartRepository.SaveCart(updatedCart);
        }

        public void AddCampaignCode(string campaignCode)
        {
            this.campaignCode = campaignCode;

            var updatedCart = cartPricingProvider.CalculatePricing(this);
            cartRepository.SaveCart(updatedCart);
        }

        public void RemoveCampaignCode()
        {
            this.campaignCode = string.Empty;

            var updatedCart = cartPricingProvider.CalculatePricing(this);
            cartRepository.SaveCart(updatedCart);
        }

        public void Clear()
        {
            this.cartItems.Clear();
            this.campaignCode = string.Empty;
            this.SubTotal = 0m;
            this.Tax = 0m;
            this.Total = 0m;

            cartRepository.SaveCart(this);
        }
    }
}
