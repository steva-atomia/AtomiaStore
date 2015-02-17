using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Web.Plugin.Validation.HtmlHelpers;
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
