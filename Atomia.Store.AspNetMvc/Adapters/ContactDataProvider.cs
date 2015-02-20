using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;
using System.Web;

namespace Atomia.Store.AspNetMvc.Adapters
{
    public class ContactDataProvider : IContactDataProvider
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
