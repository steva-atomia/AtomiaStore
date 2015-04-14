using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Item that can be added to <see cref="Cart"/>.
    /// </summary>
    public sealed class CartItem
    {
        private Guid id;

        private decimal quantity;
        private decimal price;
        private decimal discount;
        private List<Tax> taxes = new List<Tax>();

        /// <summary>
        /// The article number of the item
        /// </summary>
        public string ArticleNumber { get; set; }

        /// <summary>
        /// The quantity of the item. Must be greater than 0.
        /// </summary>
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

        /// <summary>
        /// <see cref="RenewalPeriod"/>
        /// </summary>
        public RenewalPeriod RenewalPeriod { get; set; }

        /// <summary>
        /// List of <see cref="CustomAttribute"/>
        /// </summary>
        public List<CustomAttribute> CustomAttributes { get; set; }

        /// <summary>
        /// Id assigned to the item when added to <see cref="Atomia.Store.Core.Cart"/>
        /// </summary>
        public Guid Id
        {
            get
            {
                return this.id;
            }
            internal set
            {
                if (id == Guid.Empty)
                {
                    id = value;
                }
                else
                {
                    throw new InvalidOperationException("ItemId has already been set for cart item.");
                }
            }
        }

        /// <summary>
        /// Item price, set via <see cref="SetPricing"/>
        /// </summary>
        public decimal Price
        {
            get
            {
                return price;
            }
        }

        /// <summary>
        /// Item discount, set via <see cref="SetPricing"/>
        /// </summary>
        public decimal Discount
        {
            get
            {
                return discount;
            }
        }

        /// <summary>
        /// Taxes, set via <see cref="SetPricing"/>
        /// </summary>
        public ICollection<Tax> Taxes
        {
            get
            {
                return taxes;
            }
        }

        /// <summary>
        /// Item total, calculated from values set via <see cref="SetPricing"/>
        /// </summary>
        public decimal Total
        {
            get 
            {
                return (price - discount) * quantity;
            }
        }

        /// <summary>
        /// Set price, discount and tax of item, which must all be greater than 0.
        /// </summary>
        /// <param name="price">The price to set</param>
        /// <param name="discount">The discount to set</param>
        /// <param name="taxes">The taxes to set.</param>
        public void SetPricing(decimal price, decimal discount, IEnumerable<Tax> taxes)
        {
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException("price");
            }

            if (discount < 0)
            {
                throw new ArgumentOutOfRangeException("discount");
            }

            if (taxes == null)
            {
                throw new ArgumentNullException("taxes");
            }

            this.price = price;
            this.discount = discount;
            this.taxes = taxes.ToList();
        }
    }
}
