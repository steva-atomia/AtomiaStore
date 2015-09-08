using Atomia.Store.Core;
using System;
using System.Collections.Generic;

namespace Atomia.Store.PublicBillingApi.Handlers
{
    /// <summary>
    /// Combination of data from <see cref="Atomia.Store.Core.CartItem"/>, <see cref="Atomia.Store.Core.Product"/> and renewal period
    /// </summary>
    public sealed class ItemData
    {
        private readonly Product product;
        private readonly CartItem cartItem;
        private readonly Guid renewalPeriodId;

        /// <summary>
        /// Create new instance
        /// </summary>
        public ItemData(CartItem cartItem, Product product, Guid renewalPeriodId)
        {
            if (cartItem == null)
            {
                throw new ArgumentNullException("cartItem");
            }

            if (product == null)
            {
                throw new ArgumentNullException("product");
            }

            this.cartItem = cartItem;
            this.product = product;
            this.renewalPeriodId = renewalPeriodId;
        }

        /// <summary>
        /// The cart item
        /// </summary>
        public CartItem CartItem
        {
            get
            {
                return cartItem;
            }
        }

        /// <summary>
        /// The product
        /// </summary>
        public Product Product
        {
            get
            {
                return product;
            }
        }

        /// <summary>
        /// Shortcut for article number.
        /// </summary>
        public string ArticleNumber
        {
            get
            {
                return cartItem.ArticleNumber;
            }
        }

        /// <summary>
        /// Shortcut for category.
        /// </summary>
        public IEnumerable<Category> Categories
        {
            get
            {
                return product.Categories;
            }
        }

        /// <summary>
        /// Renewal period id.
        /// </summary>
        public Guid RenewalPeriodId
        {
            get
            {
                return renewalPeriodId;
            }
        }

        /// <summary>
        /// Shortcut for quantity
        /// </summary>
        public decimal Quantity
        {
            get
            {
                return cartItem.Quantity;
            }
        }
    }
}
