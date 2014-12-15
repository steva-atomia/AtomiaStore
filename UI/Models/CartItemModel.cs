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
    public class CartItemModel : IPresentableItem
    {
        private readonly IItemPresenter itemPresenter;
        private readonly ICurrencyFormatter currencyFormatter;
        private CartItem cartItem;
       
        public CartItemModel()
        {
            itemPresenter = DependencyResolver.Current.GetService<IItemPresenter>();
            currencyFormatter = DependencyResolver.Current.GetService<ICurrencyFormatter>();
            cartItem = new CartItem();
        }

        internal CartItem CartItem
        {
            get
            {
                return cartItem;
            }
            set
            {
                this.cartItem = value;
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
                return new RenewalPeriodModel
                {
                    Period = cartItem.RenewalPeriod.Period,
                    Unit = cartItem.RenewalPeriod.Unit
                };
            }
            set
            {
                cartItem.RenewalPeriod.Period = value.Period;
                cartItem.RenewalPeriod.Unit = value.Unit;
            }
        }

        public List<CustomAttribute> CustomAttributes
        {
            get
            {
                return cartItem.CustomAttributes;
            }
            set
            {
                cartItem.CustomAttributes = value;
            }
        }

        public int Id
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
