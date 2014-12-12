using System;

namespace Atomia.Store.Core
{
    public sealed class CartItem : Item
    {
        private int id;
        private readonly string articleNumber;

        private decimal quantity;
        private decimal discount;
        private decimal taxAmount;

        public CartItem(string articleNumber, decimal quantity, IItemDisplayProvider itemDisplayProvider, ICurrencyProvider currencyProvider) :
            base(itemDisplayProvider, currencyProvider)
        {
            if (string.IsNullOrEmpty(articleNumber))
            {
                throw new ArgumentException("articleNumber");
            }

            Quantity = quantity;

            this.articleNumber = articleNumber;
        }

        public override string ArticleNumber
        {
            get
            {
                return articleNumber;
            }
        }

        public decimal Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Quantity must be greater than 0.");
                }

                quantity = value;
            }
        }

        public RenewalPeriod RenewalPeriod { get; set; }

        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                if (id == 0)
                {
                    id = value;
                }
                else
                {
                    throw new InvalidOperationException("ItemId has already been set for cart item.");
                }
            }
        }

        public decimal Price
        {
            get
            {
                return RenewalPeriod.Price;
            }
        }

        public decimal Discount
        {
            get
            {
                return discount;
            }
        }

        public decimal TaxAmount
        {
            get 
            { 
                return taxAmount; 
            }
        }

        public decimal Total
        {
            get 
            { 
                return (RenewalPeriod.Price - Discount) * Quantity; 
            }
        }

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

            this.RenewalPeriod.Price = price;
            this.discount = discount;
            this.taxAmount = taxAmount;
        }

        public string DisplayPrice
        {
            get
            {
                return currencyProvider.FormatAmount(RenewalPeriod.Price);
            }
        }

        public string DisplayDiscount
        {
            get
            {
                return currencyProvider.FormatAmount(Discount);
            }
        }

        public string DisplayTaxAmount
        {
            get
            {
                return currencyProvider.FormatAmount(TaxAmount);
            }
        }

        public string DisplayTotal
        {
            get
            {
                return currencyProvider.FormatAmount(Total);
            }
        }
    }
}
