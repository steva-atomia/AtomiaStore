using Atomia.Common;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System.Linq;

namespace Atomia.Store.PublicOrderHandlers.ContactDataHandlers
{
    /// <summary>
    /// Handler to amend order with main contact data collected from customer.
    /// </summary>
    public class MainContactDataHandler : OrderDataHandler
    {
        /// <summary>
        /// Amend order with main contact data collected from customer.
        /// </summary>
        public override PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext)
        {
            var mainContact = orderContext.ContactData.OfType<MainContactModel>().First();

            order.Email = Normalize(mainContact.Email);

            order.FirstName = Normalize(mainContact.FirstName);
            order.LastName = Normalize(mainContact.LastName);

            order.Address = Normalize(mainContact.Address);
            order.Address2 = Normalize(mainContact.Address2);
            order.Zip = Normalize(mainContact.Zip);
            order.City = Normalize(mainContact.City);
            order.Country = Normalize(mainContact.Country);

            var fax = FormattingHelper.FormatPhoneNumber(mainContact.Fax, mainContact.Country);
            order.Fax = Normalize(fax);

            var phone = FormattingHelper.FormatPhoneNumber(mainContact.Phone, mainContact.Country);
            order.Phone = Normalize(phone);

            if (mainContact.CustomerType == "individual" && mainContact.IndividualInfo != null)
            {
                order.CompanyNumber = Normalize(mainContact.IndividualInfo.IdentityNumber);
            }
            else if (mainContact.CustomerType == "company" && mainContact.CompanyInfo != null)
            {
                order.Company = Normalize(mainContact.CompanyInfo.CompanyName);
                order.CompanyNumber = Normalize(mainContact.CompanyInfo.IdentityNumber);
                order.LegalNumber = Normalize(mainContact.CompanyInfo.VatNumber);
            }

            return order;
        }
    }
}
