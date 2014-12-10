using System;
using System.Collections.Generic;


namespace Atomia.Store.Core
{
    public sealed class CartItem
    {
        private readonly IProductNameProvider productNameProvider;
        private readonly string articleNumber;

        private decimal quantity;
        private int? id;
        private decimal price;
        private decimal discount;
        private decimal taxAmount;

        public CartItem(IProductNameProvider productProvider, string articleNumber)
        {
            if (productProvider == null)
            {
                throw new ArgumentNullException("productProvider");
            }

            if (string.IsNullOrEmpty(articleNumber))
            {
                throw new ArgumentException("articleNumber");
            }

            this.productNameProvider = productProvider;
            this.articleNumber = articleNumber;
        }

        public string Name
        {
            get
            {
                return productNameProvider.GetProductName(this.articleNumber);
            }
        }

        public string ArticleNumber { get { return articleNumber; } }

        public decimal Quantity { 
            get
            {
                return this.quantity;
            }

            set 
            {
                if (value <= 0)
                {
                    throw new ArgumentException();
                }

                this.quantity = value;
            }
        }

        public RenewalPeriod RenewalPeriod { get; set; }

        public int? Id { 
            get 
            {
                return this.id;
            }
            set
            {
                if (id == null)
                {
                    id = value;
                }
                else
                {
                    throw new InvalidOperationException("ItemId has already been set for cart item.");
                }
            }
        }

        public decimal Price { get { return price; } }

        public decimal Discount { get { return discount; } }

        public decimal TaxAmount { get { return taxAmount; } }
        
        public decimal Total
        {
            get { return (Price - Discount) * Quantity; }
        }

        public Dictionary<string, string> CustomAttributes { get; set; }

        public void SetPricing(decimal price, decimal discount, decimal taxAmount)
        {
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException("price");
            }

            if (discount < 0)
            {
                throw new ArgumentOutOfRangeException("discount");
            }

            if (taxAmount < 0)
            {
                throw new ArgumentOutOfRangeException("taxAmount");
            }

            this.price = price;
            this.discount = discount;
            this.taxAmount = taxAmount;
        }
    }
}
