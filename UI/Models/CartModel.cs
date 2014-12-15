using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Atomia.Store.Core;
using Atomia.Store.AspNetMvc.Services;

namespace Atomia.Store.AspNetMvc.Models
{
    public class CartModel
    {
        private readonly ICurrencyFormatter currencyFormatter;
        private readonly Cart cart;

        public CartModel(Cart cart)
        {
            this.cart = cart;
            this.currencyFormatter = DependencyResolver.Current.GetService<ICurrencyFormatter>();
        }

        public IReadOnlyCollection<CartItemModel> CartItems
        {
            get
            {
                return cart.CartItems.Select(ci => new CartItemModel { CartItem = ci }).ToList();
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
