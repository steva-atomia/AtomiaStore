using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.PublicBillingApi;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Common;

namespace Atomia.Store.PublicOrderHandlers.ContactDataHandlers
{
    public class MainContactDataHandler : OrderDataHandler
    {
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
            order.Phone = Normalize(mainContact.Phone);

            if (mainContact.IndividualInfo != null)
            {
                order.CompanyNumber = Normalize(mainContact.IndividualInfo.IdentityNumber);
            }
            else if (mainContact.CompanyInfo != null)
            {
                order.Company = Normalize(mainContact.CompanyInfo.CompanyName);
                order.CompanyNumber = Normalize(mainContact.CompanyInfo.IdentityNumber);
                order.LegalNumber = Normalize(mainContact.CompanyInfo.VatNumber);
            }

            return order;
        }
    }
}
