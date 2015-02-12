using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;
using System.Web;
using Atomia.Store.AspNetMvc.Helpers;

namespace Atomia.Store.AspNetMvc.Adapters
{
    public class ResellerIdentifierProvider : IResellerIdentifierProvider
    {
        public ResellerIdentifier GetResellerIdentifier()
        {
            var identifier = HttpContext.Current.Session["ResellerIdentifier"] as ResellerIdentifier;

            if (identifier == null)
            {
                identifier = new ResellerIdentifier
                {
                    AccountHash = "",
                    BaseUrl = BaseUriHelper.GetBaseUriString()
                };
            }

            return identifier;
        }

        public void SetResellerIdentifier(ResellerIdentifier identifier)
        {
            HttpContext.Current.Session["ResellerIdentifier"] = identifier;
        }
    }
}
