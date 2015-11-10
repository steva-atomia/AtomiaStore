using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Shopping cart
    /// </summary>
    public sealed class Cart
    {
        private readonly ICartProvider cartProvider;
        private readonly ICartPricingService cartPricingService;
        
        private List<CartItem> cartItems = new List<CartItem>();
        private string campaignCode = String.Empty;
        private decimal subTotal;
        private decimal total;
        private List<Tax> taxes = new List<Tax>();
        private List<CustomAttribute> customAttribues = new List<CustomAttribute>();

        /// <summary>
        /// Cart constructor
        /// </summary>
        /// <param name="cartProvider">Provider to get current cart.</param>
        /// <param name="cartPricingService">Service to set price on cart and items.</param>
        public Cart(ICartProvider cartProvider, ICartPricingService cartPricingService)
        {
            if (cartProvider == null)
            {
                throw new ArgumentNullException("cartProvider");
            }

            if (cartPricingService == null)
            {
                throw new ArgumentNullException("cartPricingService");
            }

            this.cartProvider = cartProvider;
            this.cartPricingService = cartPricingService;
        }

        /// <summary>
        /// Items in cart.
        /// </summary>
        public ICollection<CartItem> CartItems 
        { 
            get 
            { 
                return this.cartItems; 
            } 
        }

        /// <summary>
        /// Attributes on cart.
        /// </summary>
        public ICollection<CustomAttribute> CustomAttributes
        {
            get
            {
                return this.customAttribues;
            }
        }

        /// <summary>
        /// Campaign code in cart.
        /// </summary>
        public string CampaignCode 
        { 
            get 
            { 
                return campaignCode;
            }
        }

        /// <summary>
        /// Cart subtotal.
        /// </summary>
        public decimal SubTotal 
        { 
            get 
            { 
                return subTotal; 
            } 
        }

        /// <summary>
        /// Cart tax.
        /// </summary>
        public ICollection<Tax> Taxes
        { 
            get 
            { 
                return taxes; 
            } 
        }

        /// <summary>
        /// Cart total.
        /// </summary>
        public decimal Total 
        { 
            get 
            { 
                return total; 
            } 
        }

        /// <summary>
        /// Set cart pricing totals.
        /// </summary>
        /// <param name="subTotal">Cart subtotal</param>
        /// <param name="total">Cart total</param>
        /// <param name="taxes">Cart taxes</param>
        public void SetPricing(decimal subTotal, decimal total, IEnumerable<Tax> taxes)
        {
            if (subTotal < 0)
            {
                throw new ArgumentOutOfRangeException("subTotal");
            }

            if (total < 0)
            {
                throw new ArgumentOutOfRangeException("total");
            }

            if (taxes == null) {
                throw new ArgumentNullException("taxes");
            }

            this.subTotal = subTotal;
            this.total = total;
            this.taxes = taxes.ToList();
        }

        /// <summary>
        /// Add multiple cart items and campaign code to cart.
        /// </summary>
        /// <param name="cartItems">Cart items to add</param>
        /// <param name="campaignCode">Campaign code to add</param>
        public void UpdateCart(IEnumerable<CartItem> cartItems, string campaignCode)
        {
            if (cartItems == null)
            {
                throw new ArgumentNullException("cartItems");
            }

            foreach(var cartItem in cartItems)
            {
                cartItem.Id = Guid.NewGuid();
                this.cartItems.Add(cartItem);
            }

            if (String.IsNullOrEmpty(campaignCode))
            {
                this.campaignCode = String.Empty;
            }
            else
            {
                this.campaignCode = campaignCode;
            }

            RecalculatePricingAndSave();
        }

        /// <summary>
        /// Add a single cart item to cart.
        /// </summary>
        /// <param name="cartItem">The cart item to add.</param>
        /// <returns>The id assigned to the cart item.</returns>
        public Guid AddItem(CartItem cartItem)
        {
            if (cartItem == null)
            {
                throw new ArgumentNullException("cartItem");
            }
            
            cartItem.Id = Guid.NewGuid();
            this.cartItems.Add(cartItem);


            try
            {
                RecalculatePricingAndSave();
            }
            catch(Exception)
            {
                this.cartItems.Remove(cartItem);
                throw;
            }
            

            return cartItem.Id;
        }

        /// <summary>
        /// Remove item matching id
        /// </summary>
        /// <param name="itemId">Id of the cart item to remove</param>
        public void RemoveItem(Guid itemId)
        {
            var cartItem = this.cartItems.Find(x => x.Id == itemId);

            if (cartItem == default(CartItem))
            {
                throw new ArgumentException("itemId");
            }

            this.cartItems.Remove(cartItem);

            RecalculatePricingAndSave();
        }

        /// <summary>
        /// Set campaign code in cart.
        /// </summary>
        /// <remarks>Adding an empty string is equal to removing the campaign code</remarks>
        /// <param name="campaignCode">The campaign code to set</param>
        public void SetCampaignCode(string campaignCode)
        {
            if (campaignCode == null)
            {
                throw new ArgumentNullException("campaignCode");
            }

            this.campaignCode = campaignCode;
            RecalculatePricingAndSave();
        }

        /// <summary>
        /// Remove campaign code from cart.
        /// </summary>
        public void RemoveCampaignCode()
        {
            this.campaignCode = string.Empty;
            RecalculatePricingAndSave();
        }

        /// <summary>
        /// Set custom attribute on cart.
        /// </summary>
        /// <param name="name">The name of the custom attribute to set</param>
        /// <param name="value">The value of the custom attribute to set</param>
        public void SetCustomAttribute(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            this.customAttribues.Add(new CustomAttribute { Name = name, Value = value });
        }

        /// <summary>
        /// Set custom attribute on an item in cart. Will add or override attribute if exists.
        /// </summary>
        /// <param name="itemId">Id of the cart item to set custom attribute on</param>
        /// <param name="name">The name of the custom attribute to set</param>
        /// <param name="value">The value of the custom attribute to set</param>
        public void SetItemAttribute(Guid itemId, string name, string value)
        {
            var cartItem = this.cartItems.Find(x => x.Id == itemId);

            if (cartItem == default(CartItem))
            {
                throw new ArgumentException("itemId");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            var attributeToSet = cartItem.CustomAttributes.FirstOrDefault(ca => ca.Name == name);
            
            if (attributeToSet == null) {
                cartItem.CustomAttributes.Add(new CustomAttribute
                {
                    Name = name,
                    Value = value
                });
            }
            else
            {
                attributeToSet.Value = value;
            }

            RecalculatePricingAndSave();
        }

        /// <summary>
        /// Remove custom attribute from item in cart.
        /// </summary>
        /// <param name="itemId">Id of the item to remove custom attribute from</param>
        /// <param name="name">Name of the custom attribute to remove.</param>
        public void RemoveItemAttribute(Guid itemId, string name)
        {
            var cartItem = this.cartItems.Find(x => x.Id == itemId);

            if (cartItem == default(CartItem))
            {
                throw new ArgumentException("itemId");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name");
            }

            var attributeToRemove = cartItem.CustomAttributes.FirstOrDefault(ca => ca.Name == name);

            if (attributeToRemove != null)
            {
                cartItem.CustomAttributes.Remove(attributeToRemove);
                RecalculatePricingAndSave();                
            }

        }

        /// <summary>
        /// Change quantity of item in cart
        /// </summary>
        /// <param name="itemId">Id of the item to remove custom attribute from</param>
        /// <param name="newQuantity">The quantity to set on the item</param>
        public void ChangeQuantity(Guid itemId, decimal newQuantity)
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

        /// <summary>
        /// Remove all cart items and any campaign code from cart.
        /// </summary>
        public void Clear()
        {
            this.cartItems.Clear();
            this.customAttribues.Clear();
            this.campaignCode = string.Empty;
            this.SetPricing(0m, 0m, new List<Tax>());
            cartProvider.SaveCart(this);
        }

        /// <summary>
        /// Check if cart is empty or not
        /// </summary>
        public bool IsEmpty()
        {
            return cartItems.Count == 0;
        }

        private void RecalculatePricingAndSave()
        {
            var updatedCart = cartPricingService.CalculatePricing(this);
            cartProvider.SaveCart(updatedCart);
        }
    }
}
