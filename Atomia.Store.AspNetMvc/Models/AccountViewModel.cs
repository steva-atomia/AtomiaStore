using Atomia.Store.Core;
using System.Collections.Generic;

namespace Atomia.Store.AspNetMvc.Models
{
    public abstract class AccountViewModel : IContactDataCollection
    {
        public abstract IEnumerable<ContactData> GetContactData();
    }


    public class DefaultAccountViewModel : AccountViewModel
    {
        public DefaultAccountViewModel()
        {
            MainContact = new MainContactModel();
            BillingContact = new BillingContactModel();
        }

        public MainContactModel MainContact { get; set; }

        public BillingContactModel BillingContact { get; set; }

        public bool OtherBillingContact { get; set; }

        public override IEnumerable<ContactData> GetContactData()
        {
            return new ContactData[] {
                MainContact,
                BillingContact
            };
        }
    }
}
