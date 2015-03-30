using Atomia.Store.Core;
using System.Web;

namespace Atomia.Store.AspNetMvc.Adapters
{
    /// <summary>
    /// Session backed <see cref="Atomia.Store.Core.IContactDataProvider"/>
    /// </summary>
    public sealed class ContactDataProvider : IContactDataProvider
    {
        /// <inheritdoc />
        public IContactDataCollection GetContactData()
        {
            return HttpContext.Current.Session["ContactData"] as IContactDataCollection;
        }

        /// <inheritdoc />
        public void SaveContactData(IContactDataCollection contactData)
        {
            HttpContext.Current.Session["ContactData"] = contactData;
        }

        /// <inheritdoc />
        public void ClearContactData()
        {
            HttpContext.Current.Session["ContactData"] = null;
        }
    }
}
