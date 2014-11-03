using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Atomia.OrderPage.Core.Models
{
    public abstract class CheckoutModel
    {
        public virtual ContactInfo MainContact { get; set; }
    }
    

    public class ContactInfo
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        public virtual string OrganizationName { get; set; }

        public virtual string Address { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string Country { get; set; }

        public virtual string Email { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Fax { get; set; }
    }
}
