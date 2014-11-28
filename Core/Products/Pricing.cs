using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core.Products
{
    public class Pricing
    {
        private readonly decimal price;
        private readonly decimal discount;
        private readonly decimal tax;
        private readonly decimal amount;
                          
        public Pricing(decimal price, decimal discount, decimal tax, decimal amount)
        {
            this.price = price;
            this.discount = discount;
            this.tax = tax;
            this.amount = amount;
        }

        public decimal Price { get { return price; } }

        public decimal Discount { get { return discount; } }

        public decimal Tax { get { return tax; } }

        public decimal Amount { get { return amount; } }
    }
}
