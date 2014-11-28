using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atomia.Store.Core.Products;
// NOTE: Need to keep it with default constructor and readwrite properties for asp.net mvc model binder to work.

namespace Atomia.Store.Core.Cart
{
    public class CartItem
    {
        public virtual string Name { get; set; }

        public virtual string ArticleNumber { get; set; }

        public virtual int Quantity { get; set; }

        public virtual RenewalPeriod RenewalPeriod { get; set; }

        public virtual Pricing Pricing { get; set; }
    }
}
