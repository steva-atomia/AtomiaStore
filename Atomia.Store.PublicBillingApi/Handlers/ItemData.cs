using Atomia.Store.Core;
using System;

namespace Atomia.Store.PublicBillingApi.Handlers
{
    public sealed class ItemData
    {
        private readonly Product product;
        private readonly CartItem cartItem;
        private readonly Guid renewalPeriodId;

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

        public CartItem CartItem
        {
            get
            {
                return cartItem;
            }
        }

        public Product Product
        {
            get
            {
                return product;
            }
        }

        public string ArticleNumber
        {
            get
            {
                return cartItem.ArticleNumber;
            }
        }

        public string Category
        {
            get
            {
                return product.Category;
            }
        }

        public Guid RenewalPeriodId
        {
            get
            {
                return renewalPeriodId;
            }
        }

        public decimal Quantity
        {
            get
            {
                return cartItem.Quantity;
            }
        }
    }
}
