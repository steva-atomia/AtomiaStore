using System;
using System.Web;
using System.Web.Mvc;
using Atomia.Store.Core;

namespace Atomia.Store.WebBase.HtmlHelpers
{
    public static class CommonResourcesHelper
    {
        private static IThemeNamesProvider themeNamesProvider = DependencyResolver.Current.GetService<IThemeNamesProvider>();

        public static string CommonResource(this HtmlHelper htmlhelper, string expression, params object[] args)
        {
            var themeClass = themeNamesProvider.GetMainThemeName() + "Common,";
            var resource = LocalizationHelpers.Resource(htmlhelper, themeClass + expression, args);

            return String.IsNullOrEmpty(resource)
                ? LocalizationHelpers.Resource(htmlhelper, "Common," + expression, args)
                : resource;
        }

        public static IHtmlString CommonResourceRaw(this HtmlHelper htmlhelper, string expression, params object[] args)
        {
            var themeClass = themeNamesProvider.GetMainThemeName() + "Common,";
            var resource = LocalizationHelpers.Resource(htmlhelper, themeClass + expression, args);

            if (String.IsNullOrEmpty(resource))
            {
                resource = LocalizationHelpers.Resource(htmlhelper, "Common," + expression, args);
            }

            return htmlhelper.Raw(resource);
        }

        public static string CommonResourceJs(this HtmlHelper htmlhelper, string expression, params object[] args)
        {
            var themeClass = themeNamesProvider.GetMainThemeName() + "Common,";
            var resource = LocalizationHelpers.ResourceJavascript(htmlhelper, themeClass + expression, args);

            return String.IsNullOrEmpty(resource)
                ? LocalizationHelpers.ResourceJavascript(htmlhelper, "Common," + expression, args)
                : resource;
        }
    }
}
