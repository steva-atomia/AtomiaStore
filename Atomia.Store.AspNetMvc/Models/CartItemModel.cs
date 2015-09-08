using Atomia.Store.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// View model for <see cref="Atomia.Store.Core.CartItem"/> with values formatted and localized for display.
    /// </summary>
    public class CartItemModel : IPresentableItem
    {
        private readonly IItemPresenter itemPresenter = DependencyResolver.Current.GetService<IItemPresenter>();
        private readonly ICurrencyFormatter currencyFormatter = DependencyResolver.Current.GetService<ICurrencyFormatter>();

        private CartItem cartItem;
       
        public CartItemModel()
        {
            if (this.cartItem == null)
            {
                this.cartItem = new CartItem();
            }
        }

        public CartItemModel(CartItem cartItem) : this()
        {
            this.cartItem = cartItem;
        }

        /// <summary>
        /// The represented <see cref="Atomia.Store.Core.CartItem"/>
        /// </summary>
        internal CartItem CartItem
        {
            get
            {
                return cartItem;
            }
        }

        public string ArticleNumber
        {
            get
            {
                return cartItem.ArticleNumber;
            }
            set
            {
                cartItem.ArticleNumber = value;
            }
        }

        public decimal Quantity
        {
            get
            {
                return cartItem.Quantity;
            }
            set
            {
                cartItem.Quantity = value;
            }
        }

        public RenewalPeriodModel RenewalPeriod {
            get
            {
                return cartItem.RenewalPeriod != null ? new RenewalPeriodModel
                {
                    Period = cartItem.RenewalPeriod.Period,
                    Unit = cartItem.RenewalPeriod.Unit
                } : null;
            }
            set
            {
                if (value != null)
                {
                    cartItem.RenewalPeriod = new RenewalPeriod(value.Period, value.Unit);
                }
            }
        }

        public List<CustomAttribute> CustomAttributes
        {
            get
            {
                if (cartItem.CustomAttributes == null)
                {
                    cartItem.CustomAttributes = new List<CustomAttribute>();
                }

                return cartItem.CustomAttributes;
            }
            set
            {
                cartItem.CustomAttributes = value;
            }
        }

        public Guid Id
        {
            get
            {
                return cartItem.Id;
            }
        }

        public string Name
        {
            get
            {
                return itemPresenter.GetName(this);
            }
        }

        public string Description
        {
            get
            {
                return itemPresenter.GetDescription(this);
            }
        }

        public IEnumerable<Category> Categories
        {
            get
            {
                return itemPresenter.GetCategories(this);
            }
        }

        public string Price
        {
            get
            {
                return currencyFormatter.FormatAmount(cartItem.Price);
            }
        }

        public IReadOnlyCollection<TaxModel> Taxes
        {
            get
            {
                return cartItem.Taxes.Select(t => new TaxModel(t)).ToList();
            }
        }

        public string Discount
        {
            get
            {
                return currencyFormatter.FormatAmount(cartItem.Discount);
            }
        }

        public string Total
        {
            get
            {
                return currencyFormatter.FormatAmount(cartItem.Total);
            }
        }
    }
}
