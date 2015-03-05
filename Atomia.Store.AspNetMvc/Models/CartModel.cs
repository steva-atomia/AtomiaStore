using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Models
{
    public class CartModel
    {
        private readonly ICurrencyFormatter currencyFormatter = DependencyResolver.Current.GetService<ICurrencyFormatter>();

        private readonly Cart cart;

        public CartModel(Cart cart)
        {
            this.cart = cart;
        }

        public IReadOnlyCollection<CartItemModel> CartItems
        {
            get
            {
                return cart.CartItems.Select(ci => new CartItemModel(ci)).ToList();
            }
        }

        public string CampaignCode
        {
            get
            {
                return cart.CampaignCode;
            }
        }

        public string SubTotal
        {
            get
            {
                return currencyFormatter.FormatAmount(cart.SubTotal);
            }
        }

        public string Tax
        {
            get
            {
                return currencyFormatter.FormatAmount(cart.Tax);
            }
        }

        public string Total
        {
            get
            {
                return currencyFormatter.FormatAmount(cart.Total);
            }
        }
    }
}
