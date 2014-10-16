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

    public class ContactInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string OrganizationName { get; set; }

        public string Address { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
}