using Atomia.Store.Core;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Filters
{
    /// <summary>
    /// Filter for setting theme based on query string "?theme={themeName}".
    /// </summary>
    public sealed class ThemeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            if (filterContext.Controller.ControllerContext.IsChildAction || request.IsAjaxRequest())
            {
                return;
            }

            var themeName = request.QueryString["theme"] as string;

            if (themeName != null)
            {
                var themeNamesProvider = DependencyResolver.Current.GetService<IThemeNamesProvider>();

                themeNamesProvider.SetCurrentThemeName(themeName);
            }
        }
    }
}
