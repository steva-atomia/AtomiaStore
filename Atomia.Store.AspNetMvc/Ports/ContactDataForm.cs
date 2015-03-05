using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Ports
{
    public abstract class ContactDataForm : ContactData
    {
        public virtual string PartialViewName
        {
            get
            {
                return "_" + this.Id;
            }
        }
    }
}
