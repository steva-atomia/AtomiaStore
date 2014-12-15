using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core
{
    public class CartItemProvider : ICartItemProvider
    {
        private readonly IItemDisplayProvider itemDisplayProvider;
        private readonly ICurrencyProvider currencyProvider;

        public CartItemProvider(IItemDisplayProvider itemDisplayProvider, ICurrencyProvider currencyProvider)
        {
            if (itemDisplayProvider == null)
            {
                throw new ArgumentNullException("itemDisplayProvider");
            }

            if (currencyProvider == null)
            {
                throw new ArgumentNullException("currencyProvider");
            }

            this.itemDisplayProvider = itemDisplayProvider;
            this.currencyProvider = currencyProvider;
        }

        public CartItem CreateCartItem(string articleNumber, decimal quantity)
        {
            return new CartItem(articleNumber, quantity, itemDisplayProvider, currencyProvider);
        }
    }
}
