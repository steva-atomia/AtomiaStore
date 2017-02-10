using Atomia.Common;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.PublicBillingApi.Handlers;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace Atomia.Store.PublicOrderHandlers
{
    /// <summary>
    /// Get serialized separate domain reg contact (WHOIS) data if it has been provided by user.
    /// </summary>
    internal sealed class DomainRegContactProvider
    {
        /// <summary>
        /// Will serialize to JSON that is expected for "DomainRegContact" attribute on order item.
        /// </summary>
        private class DomainRegContact
        {
            public string City { get; set; }

            public string Country { get; set; }

            public string Email { get; set; }

            public string Fax { get; set; }

            public string Name { get; set; }

            public string Org { get; set; }

            public string OrgNo { get; set; }

            public string Street1 { get; set; }

            public string Street2 { get; set; }

            public string VatNo { get; set; }

            public string Voice { get; set; }

            public string Zip { get; set; }

            public string CustomFields { get; set; }

            public IDictionary<string, string> CustomFieldsDict { get; set; }
        }

        private string _domainRegContactData;

        public DomainRegContactProvider(PublicOrderContext orderContext)
        {
            if (orderContext == null)
            {
                throw new ArgumentNullException("orderContext");
            }

            var whoisContact = orderContext.ContactData.OfType<WhoisContactModel>().FirstOrDefault();
            var mainContact = orderContext.ContactData.OfType<MainContactModel>().FirstOrDefault();

            // No separate WHOIS contact added by customer.
            if ((whoisContact == null || String.IsNullOrEmpty(whoisContact.Email))
                &&
                (mainContact == null || mainContact.CustomFieldsInfo == null ||
                 mainContact.CustomFieldsInfo.CustomFieldsDict == null ||
                 mainContact.CustomFieldsInfo.CustomFieldsDict.Count == 0))
            {
                _domainRegContactData = "";
            }
            else
            {
                if (whoisContact == null || String.IsNullOrEmpty(whoisContact.Email))
                {
                    //whoisContact not filled out but there are custom fields in mainContact. Copy rest of the properties.
                    whoisContact = whoisContact ?? new WhoisContactModel();
                    whoisContact.Address = mainContact.Address;
                    whoisContact.Address2 = mainContact.Address2;
                    whoisContact.CartItems = mainContact.CartItems;
                    whoisContact.City = mainContact.City;
                    whoisContact.Country = mainContact.Country;
                    whoisContact.CustomerType = mainContact.CustomerType;
                    whoisContact.Email = mainContact.Email;
                    whoisContact.Fax = mainContact.Fax;
                    whoisContact.FirstName = mainContact.FirstName;
                    whoisContact.LastName = mainContact.LastName;
                    whoisContact.Phone = mainContact.Phone;
                    whoisContact.ResellerId = mainContact.ResellerId;
                    whoisContact.State = mainContact.State;
                    whoisContact.Zip = mainContact.Zip;
                    whoisContact.IndividualInfo.IdentityNumber = mainContact.IndividualInfo.IdentityNumber;
                    whoisContact.CompanyInfo.CompanyName = mainContact.CompanyInfo.CompanyName;
                    whoisContact.CompanyInfo.IdentityNumber = mainContact.CompanyInfo.IdentityNumber;
                    whoisContact.CompanyInfo.VatNumber = mainContact.CompanyInfo.VatNumber;
                }

                if (mainContact != null && mainContact.CustomFieldsInfo != null &&
                    mainContact.CustomFieldsInfo.CustomFieldsDict != null &&
                    mainContact.CustomFieldsInfo.CustomFieldsDict.Count > 0)
                {
                    whoisContact.CustomFieldsInfo.CustomFieldsDict = mainContact.CustomFieldsInfo.CustomFieldsDict;
                }
                var phone = FormattingHelper.FormatPhoneNumber(whoisContact.Phone, whoisContact.Country);
                var fax = FormattingHelper.FormatPhoneNumber(whoisContact.Fax, whoisContact.Country);

                string org = "";
                string orgNo = "";
                string vatNo = "";

                if (whoisContact.CompanyInfo != null && !string.IsNullOrEmpty(whoisContact.CompanyInfo.CompanyName))
                {
                    org = whoisContact.CompanyInfo.CompanyName;
                    orgNo = whoisContact.CompanyInfo.IdentityNumber;
                    vatNo = whoisContact.CompanyInfo.VatNumber;
                }
                else
                {
                    orgNo = whoisContact.IndividualInfo.IdentityNumber;
                }

                var domainRegContact = new DomainRegContact
                {
                    City = OrderDataHandler.NormalizeData(whoisContact.City),
                    Country = OrderDataHandler.NormalizeData(whoisContact.Country),
                    Email = OrderDataHandler.NormalizeData(whoisContact.Email),
                    Fax = OrderDataHandler.NormalizeData(fax),
                    Name =
                        OrderDataHandler.NormalizeData(whoisContact.FirstName) + " " +
                        OrderDataHandler.NormalizeData(whoisContact.LastName),
                    Org = OrderDataHandler.NormalizeData(org),
                    OrgNo = OrderDataHandler.NormalizeData(orgNo),
                    Street1 = OrderDataHandler.NormalizeData(whoisContact.Address),
                    Street2 = OrderDataHandler.NormalizeData(whoisContact.Address2),
                    VatNo = OrderDataHandler.NormalizeData(vatNo),
                    Voice = OrderDataHandler.NormalizeData(phone),
                    Zip = OrderDataHandler.NormalizeData(whoisContact.Zip),
                    CustomFields = new JavaScriptSerializer().Serialize(whoisContact.CustomFieldsInfo.CustomFieldsDict),
                    CustomFieldsDict = whoisContact.CustomFieldsInfo.CustomFieldsDict
                };

                _domainRegContactData = new JavaScriptSerializer().Serialize(domainRegContact);
            }
        }

        /// <summary>
        /// Get DomainRegContact serialized as JSON.
        /// </summary>
        public string DomainRegContactData
        {
            get { return _domainRegContactData; }
        }
    }
}
