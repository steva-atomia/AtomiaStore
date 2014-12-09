using System;
using System.Collections.Generic;

namespace Atomia.Store.Core.Cart
{
    public class Cart
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartPricingProvider cartPricingProvider;
        private string countryCode;
        private string currencyCode;
        
        private List<CartItem> cartItems = new List<CartItem>();
        private string campaignCode;
        private decimal subTotal;
        private decimal tax;
        private decimal total;
        private int itemNoCounter;

        public Cart(ICartRepository cartRepository, ICartPricingProvider cartPricingProvider, string countryCode, string currencyCode)
        {
            if (cartRepository == null)
            {
                throw new ArgumentNullException("cartRepository");
            }

            if (cartPricingProvider == null)
            {
                throw new ArgumentNullException("cartPricingProvider");
            }

            if (string.IsNullOrEmpty(countryCode))
            {
                throw new ArgumentException("countryCode");
            }

            if (string.IsNullOrEmpty(currencyCode))
            {
                throw new ArgumentException("currencyCode");
            }

            this.cartRepository = cartRepository;
            this.cartPricingProvider = cartPricingProvider;
            this.countryCode = countryCode;
            this.currencyCode = currencyCode;
        }

        public IEnumerable<CartItem> CartItems { get { return this.cartItems; } }

        public string CampaignCode { get { return campaignCode; }}

        public decimal SubTotal { get { return subTotal; } }

        public decimal Tax { get { return tax; } }

        public decimal Total { get { return total; } }

        public string CountryCode { get { return countryCode; } }

        public string CurrencyCode { get { return currencyCode; } }

        public void SetPricing(decimal subTotal, decimal tax, decimal total)
        {
            this.subTotal = subTotal;
            this.tax = tax;
            this.total = total;
        }

        public void AddItem(CartItem cartItem)
        {
            cartItem.Id = itemNoCounter++;
            this.cartItems.Add(cartItem);

            RecalculatePricingAndSave();
        }

        public void RemoveItem(int itemId)
        {
            var cartItem = this.cartItems.Find(x => x.Id == itemId);

            if (cartItem == default(CartItem))
            {
                throw new ArgumentException("itemId");
            }

            this.cartItems.Remove(cartItem);

            RecalculatePricingAndSave();
        }

        public void SetCampaignCode(string campaignCode)
        {
            this.campaignCode = campaignCode;

            RecalculatePricingAndSave();
        }

        public void RemoveCampaignCode()
        {
            this.campaignCode = string.Empty;

            RecalculatePricingAndSave();
        }

        public void ChangeQuantity(int itemId, decimal newQuantity)
        {
            if (newQuantity < 0)
            {
                throw new ArgumentOutOfRangeException("newQuantity");
            }

            var cartItem = this.cartItems.Find(x => x.Id == itemId);

            if (cartItem == default(CartItem))
            {
                throw new ArgumentException("itemId");
            }

            cartItem.Quantity = newQuantity;

            RecalculatePricingAndSave();
        }

        public void Clear()
        {
            this.cartItems.Clear();
            this.campaignCode = string.Empty;
            this.SetPricing(0m, 0m, 0m);

            cartRepository.SaveCart(this);
        }

        private void RecalculatePricingAndSave()
        {
            var updatedCart = cartPricingProvider.CalculatePricing(this);
            cartRepository.SaveCart(updatedCart);
        }
    }
}
