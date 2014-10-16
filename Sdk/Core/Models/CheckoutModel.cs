using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Atomia.OrderPage.Sdk.Core.Models
{
    public abstract class CheckoutModel
    {
        public virtual ContactInfo MainContact { get; set; }
    }
}