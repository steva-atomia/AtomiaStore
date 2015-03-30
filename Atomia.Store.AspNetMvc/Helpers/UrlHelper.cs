using System;
using System.Web;

namespace Atomia.Store.AspNetMvc.Helpers
{
    /// <summary>
    /// Some Uri helpers.
    /// </summary>
    public static class BaseUriHelper
    {
        /// <summary>
        /// Get base Uri for current request, e.g. http://www.example.com/
        /// </summary>
        /// <returns>Returns the base Uri as a <see cref="System.Uri"/>.</returns>
        public static Uri GetBaseUri()
        {
            var currentUrl = HttpContext.Current.Request.Url;
            var baseUri = new Uri(string.Format("{0}://{1}/", currentUrl.Scheme, currentUrl.Authority));

            return baseUri;
        }

        /// <summary>
        /// Get base Uri for current request, e.g. http://www.example.com/
        /// </summary>
        /// <returns>Returns the base Uri as a string.</returns>
        public static string GetBaseUriString()
        {
            var currentUrl = HttpContext.Current.Request.Url;
            var baseUri = new Uri(string.Format("{0}://{1}/", currentUrl.Scheme, currentUrl.Authority));

            return baseUri.ToString();
        }
    }
}
