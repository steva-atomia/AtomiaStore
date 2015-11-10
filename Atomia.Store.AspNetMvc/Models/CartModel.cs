using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// View model for <see cref="Atomia.Store.Core.Cart"/> with values formatted and localized for display.
    /// </summary>
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

        public IReadOnlyCollection<CustomAttribute> CustomAttributes
        {
            get
            {
                return cart.CustomAttributes.ToList();
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

        public IReadOnlyCollection<TaxModel> Taxes
        {
            get
            {
                return cart.Taxes.Select(t => new TaxModel(t)).ToList();
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
