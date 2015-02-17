using System.Collections.Generic;
using Atomia.Web.Plugin.Validation.HtmlHelpers;

namespace Atomia.Store.AspNetMvc.Models
{
    public class BillingContactModel : ContactModel
    {
        public override string Id
        {
            get { return "BillingContact"; }
        }
    }
}
