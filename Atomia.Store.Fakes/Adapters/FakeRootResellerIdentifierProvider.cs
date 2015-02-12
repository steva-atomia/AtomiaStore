using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;
using System.Web;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakeRootResellerIdentifierProvider : IResellerIdentifierProvider
    {
        public ResellerIdentifier GetResellerIdentifier()
        {
            var currentUrl = HttpContext.Current.Request.Url;
            var baseUri = new Uri(string.Format("{0}://{1}/", currentUrl.Scheme, currentUrl.Authority));

            return new ResellerIdentifier
            {
                AccountHash = "184ec886dc90d2c1b5e50b6afe80db01",
                BaseUrl = baseUri.ToString()
            };
        }
    }
}
