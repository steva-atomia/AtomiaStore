using Atomia.Store.AspNetMvc.Helpers;
using Atomia.Store.Core;
using System.Web;

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
