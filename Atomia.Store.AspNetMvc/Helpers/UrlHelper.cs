using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Atomia.Store.AspNetMvc.Helpers
{
    public static class BaseUriHelper
    {
        public static Uri GetBaseUri()
        {
            var currentUrl = HttpContext.Current.Request.Url;
            var baseUri = new Uri(string.Format("{0}://{1}/", currentUrl.Scheme, currentUrl.Authority));

            return baseUri;
        }

        public static string GetBaseUriString()
        {
            var currentUrl = HttpContext.Current.Request.Url;
            var baseUri = new Uri(string.Format("{0}://{1}/", currentUrl.Scheme, currentUrl.Authority));

            return baseUri.ToString();
        }
    }
}
