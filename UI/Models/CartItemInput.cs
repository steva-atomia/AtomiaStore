using Atomia.Store.Core;
using System.Collections.Generic;

namespace Atomia.Store.AspNetMvc.Models
{
    public class CartItemInput
    {
        public string ArticleNumber { get; set; }

        public decimal Quantity { get; set; }

        public List<CustomAttribute> CustomAttributes { get; set; }

        public RenewalPeriodInput RenewalPeriod { get; set; }

        internal CartItem ToCartItem(IItemDisplayProvider displayProvider, ICurrencyProvider currencyProvider)
        {
            return new CartItem(ArticleNumber, Quantity, displayProvider, currencyProvider)
            {
                RenewalPeriod = RenewalPeriod,
                CustomAttributes = CustomAttributes
            };
        }
    }
}
