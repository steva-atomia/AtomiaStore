using Atomia.Store.Core.Products;
using System.Collections.Generic;

namespace Atomia.Store.Core.Cart
{
    public class CartItem
    {
        public virtual string Name { get; set; }

        public virtual string ArticleNumber { get; set; }

        public virtual decimal Quantity { get; set; }

        public virtual decimal Price { get; set; }

        public virtual decimal Discount { get; set; }
        
        public virtual decimal Total
        {
            get { return (Price - Discount) * Quantity; }
        }

        public virtual decimal TaxAmount { get; set; }

        public virtual RenewalPeriod RenewalPeriod { get; set; }

        public virtual Dictionary<string, string> CustomAttributes { get; set; }
    }
}
