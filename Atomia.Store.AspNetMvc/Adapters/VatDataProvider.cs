using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Adapters
{
    public sealed class VatDataProvider : IVatDataProvider
    {
        private MainContactModel _mainContact;
        private readonly IContactDataProvider contactDataProvider = DependencyResolver.Current.GetService<IContactDataProvider>();
        
        public string VatNumber
        {
            get
            {
                if (MainContact != null && MainContact.CompanyInfo != null)
                {
                    return MainContact.CompanyInfo.VatNumber;
                }

                return String.Empty;
            }
            set
            {
                var contactData = contactDataProvider.GetContactData();
                this._mainContact = contactData.GetContactData().OfType<MainContactModel>().FirstOrDefault();

                if (this._mainContact != null && this._mainContact.CompanyInfo != null)
                {
                    this._mainContact.CompanyInfo.VatNumber = value;
                    contactDataProvider.SaveContactData(contactData);
                }
            }
        }

        public string CountryCode
        {
            get
            {
                if (MainContact != null)
                {
                    return MainContact.Country;
                }

                return String.Empty;
            }
        }

        private MainContactModel MainContact
        {
            get
            {
                if (this._mainContact == null)
                {
                    var contactDataCollection = contactDataProvider.GetContactData();
                    this._mainContact = contactDataCollection.GetContactData().OfType<MainContactModel>().FirstOrDefault();
                }

                return this._mainContact;
            }
        }
    }
}
