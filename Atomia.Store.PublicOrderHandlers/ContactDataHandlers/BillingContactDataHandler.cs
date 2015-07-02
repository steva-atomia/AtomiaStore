using Atomia.Common;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Linq;

namespace Atomia.Store.PublicOrderHandlers.ContactDataHandlers
{
    /// <summary>
    /// Handler to amend order with billing contact data collected from customer.
    /// </summary>
    public class BillingContactDataHandler : OrderDataHandler
    {
        /// <summary>
        /// Amend order with billing contact data collected from customer, falling back to collected main contact data.
        /// </summary>
        public override PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext)
        {
            var billingContact = orderContext.ContactData.OfType<BillingContactModel>().First();

            // Email is a required field on BillingContactModel,
            // so if it is not present it means no separate billing contact was provided.
            if (String.IsNullOrWhiteSpace(billingContact.Email))
            {
                var mainContact = orderContext.ContactData.OfType<MainContactModel>().First();
                
                AddContactData(order, mainContact);
            }
            else
            {
                AddContactData(order, billingContact);
            }
            
            return order;
        }

        private void AddContactData(PublicOrder order, ContactModel billingContact)
        {
            order.BillingEmail = Normalize(billingContact.Email);

            order.BillingFirstName = Normalize(billingContact.FirstName);
            order.BillingLastName = Normalize(billingContact.LastName);

            order.BillingAddress = Normalize(billingContact.Address);
            order.BillingAddress2 = Normalize(billingContact.Address2);
            order.BillingZip = Normalize(billingContact.Zip);
            order.BillingCity = Normalize(billingContact.City);
            order.BillingCountry = Normalize(billingContact.Country);

            var fax = FormattingHelper.FormatPhoneNumber(billingContact.Fax, billingContact.Country);
            order.BillingFax = Normalize(fax);

            var phone = FormattingHelper.FormatPhoneNumber(billingContact.Phone, billingContact.Country);
            order.BillingPhone = Normalize(phone);

            if (billingContact.CustomerType == "individual" && billingContact.IndividualInfo != null)
            {
                order.BillingCompanyNumber = Normalize(billingContact.IndividualInfo.IdentityNumber);
            }
            else if (billingContact.CustomerType == "company" && billingContact.CompanyInfo != null)
            {
                order.BillingCompany = Normalize(billingContact.CompanyInfo.CompanyName);
                order.BillingCompanyNumber = Normalize(billingContact.CompanyInfo.IdentityNumber);
            }
        }
    }
}
