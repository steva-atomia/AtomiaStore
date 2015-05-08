using Atomia.Store.Core;
using System.Collections.Generic;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// Abstract view model for the /Account/Index page
    /// </summary>
    public abstract class AccountViewModel : IContactDataCollection
    {
        /// <summary>
        /// Get <see cref="Atomia.Store.Core.ContactData"/> with collected information from customer.
        /// </summary>
        public abstract IEnumerable<ContactData> GetContactData();
    }


    /// <summary>
    /// The default implementation of <see cref="AccountViewModel"/>. 
    /// Collects a main contact and optionally separate billing and WHOIS contacts
    /// </summary>
    public class DefaultAccountViewModel : AccountViewModel
    {
        /// <summary>
        /// Constructor intializing the sub-forms
        /// </summary>
        public DefaultAccountViewModel()
        {
            MainContact = new MainContactModel();
            BillingContact = new BillingContactModel();
            WhoisContact = new WhoisContactModel();
        }

        /// <summary>
        /// Main contact data collected from the customer.
        /// </summary>
        public MainContactModel MainContact { get; set; }

        /// <summary>
        /// Billing contact data collected from the customer.
        /// </summary>
        public BillingContactModel BillingContact { get; set; }

        /// <summary>
        /// WHOIS contact data collected from the customer.
        /// </summary>
        public WhoisContactModel WhoisContact { get; set; }

        /// <summary>
        /// If other billing contact than main contact should be used or not.
        /// </summary>
        public bool OtherBillingContact { get; set; }

        /// <summary>
        /// If other WHOIS contact than main contact should be used or not.
        /// </summary>
        public bool OtherWhoisContact { get; set; }

        /// <summary>
        /// Get the collected contact data.
        /// </summary>
        public override IEnumerable<ContactData> GetContactData()
        {
            return new ContactData[] {
                MainContact,
                BillingContact,
                WhoisContact
            };
        }
    }
}
