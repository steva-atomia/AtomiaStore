using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.AspNetMvc.Models
{
    public abstract class ContactModel
    {
        public virtual string Email { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string CompanyName { get; set; }

        public virtual string IdentityNumber { get; set; }

        public virtual string Address { get; set; }

        public virtual string Address2 { get; set; }

        public virtual string City { get; set; }

        public virtual string Zip { get; set; }

        public virtual string Country { get; set; }

        public virtual string State { get; set; }

        public virtual string Phone { get; set; }

        public virtual string Mobile { get; set; }

        public virtual string Fax { get; set; }
    }


    public class MainContactModel : ContactModel
    {
        // Override validation to also check for username
        public override string Email { get; set; }
    }


    public class BillingContactModel : ContactModel
    {

    }
}
