using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.AspNetMvc.Models
{

    public abstract class AccountViewModel
    {
        // [Required]
        public virtual ContactModel MainContact { get; set; }
    }


    public class DefaultAccountViewModel : AccountViewModel
    {
        public DefaultAccountViewModel()
        {
            this.MainContact = new MainContactModel();
            this.BillingContact = new BillingContactModel();
        }

        public virtual string CustomerType { get; set; }

        public virtual ContactModel BillingContact { get; set; }
    }
}
