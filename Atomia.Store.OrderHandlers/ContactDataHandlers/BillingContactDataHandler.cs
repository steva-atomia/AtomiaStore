using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Common;
using Atomia.Store.PublicBillingApi;

namespace Atomia.Store.PublicOrderHandlers.ContactDataHandlers
{
    public class BillingContactDataHandler : OrderDataHandler
    {
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

            if (billingContact.IndividualInfo != null)
            {
                order.BillingCompanyNumber = Normalize(billingContact.IndividualInfo.IdentityNumber);
            }
            else if (billingContact.CompanyInfo != null)
            {
                order.BillingCompany = Normalize(billingContact.CompanyInfo.CompanyName);
                order.BillingCompanyNumber = Normalize(billingContact.CompanyInfo.IdentityNumber);
            }
        }
    }
}
