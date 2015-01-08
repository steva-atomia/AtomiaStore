using Atomia.Store.Core;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Models
{
    public class CartItemModel : IPresentableItem
    {
        private readonly IItemPresenter itemPresenter;
        private readonly ICurrencyFormatter currencyFormatter;
        private CartItem cartItem;
       
        public CartItemModel()
        {
            itemPresenter = DependencyResolver.Current.GetService<IItemPresenter>();
            currencyFormatter = DependencyResolver.Current.GetService<ICurrencyFormatter>();

            if (this.cartItem == null)
            {
                this.cartItem = new CartItem();
            }
        }

        public CartItemModel(CartItem cartItem) : this()
        {
            this.cartItem = cartItem;
        }

        internal CartItem CartItem
        {
            get
            {
                return cartItem;
            }
        }

        //[Required]
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

        //[Required]
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
                    cartItem.RenewalPeriod = new RenewalPeriod
                    {
                        Period = value.Period,
                        Unit = value.Unit
                    };
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

        public string Price
        {
            get
            {
                return currencyFormatter.FormatAmount(cartItem.Price);
            }
        }

        public string TaxAmount
        {
            get
            {
                return currencyFormatter.FormatAmount(cartItem.TaxAmount);
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
