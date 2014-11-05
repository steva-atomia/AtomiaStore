using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atomia.OrderPage.Core.Models;

namespace Atomia.OrderPage.UI.ViewModels
{
    public abstract class CheckoutViewModel
    {
        public virtual ContactInfo MainContact { get; set; }
    }
}
