using System.Web;
using System.Web.Mvc;

namespace Atomia.Store.WebBase.HtmlHelpers
{
    public static class CommonResourcesHelper
    {
        public static string CommonResource(this HtmlHelper htmlhelper, string expression, params object[] args)
        {
            return LocalizationHelpers.Resource(htmlhelper, "Common," + expression, args);
        }

        public static IHtmlString CommonResourceRaw(this HtmlHelper htmlhelper, string expression, params object[] args)
        {
            var s = LocalizationHelpers.Resource(htmlhelper, "Common," + expression, args);

            return htmlhelper.Raw(s);
        }

        public static string CommonResourceJs(this HtmlHelper htmlhelper, string expression, params object[] args)
        {
            return LocalizationHelpers.ResourceJavascript(htmlhelper, "Common," + expression, args);
        }
    }
}
