using Atomia.Store.Core;
using System.Web;

namespace Atomia.Store.AspNetMvc.Adapters
{
    public sealed class ContactDataProvider : IContactDataProvider
    {
        public IContactDataCollection GetContactData()
        {
            return HttpContext.Current.Session["ContactData"] as IContactDataCollection;
        }

        public void SaveContactData(IContactDataCollection contactData)
        {
            HttpContext.Current.Session["ContactData"] = contactData;
        }

        public void ClearContactData()
        {
            HttpContext.Current.Session["ContactData"] = null;
        }
    }
}
