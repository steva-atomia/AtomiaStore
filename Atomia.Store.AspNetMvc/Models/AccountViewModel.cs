using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Atomia.Store.Core;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Models
{

    public abstract class AccountViewModel
    {
        
    }


    public sealed class DefaultAccountViewModel : AccountViewModel
    {
        public DefaultAccountViewModel()
        {
            var resellerProvider = DependencyResolver.Current.GetService<IResellerProvider>();
            var cartProvider = DependencyResolver.Current.GetService<ICartProvider>();

            var resellerId = resellerProvider.GetResellerId();
            var cart = cartProvider.GetCart();


            // Initialize contact submodels with properties required by Atomia Validation plugin.

            this.MainContact = new MainContactModel()
            {
                ResellerId = resellerId,
                CartItems = cart.CartItems.ToList()
            };

            this.BillingContact = new BillingContactModel()
            {
                ResellerId = resellerId,
                CartItems = cart.CartItems.ToList()
            };
        }

        public string CustomerType { get; set; }

        public MainContactModel MainContact { get; set; }

        public BillingContactModel BillingContact { get; set; }
    }
}
