using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Atomia.Store.AspNetMvc.HtmlHelpers
{
    /// <summary>
    /// Html helper extensions for including JSON controller action results server-side in view.
    /// </summary>
    public static class JsonActionHelper
    {
        /// <summary>
        /// Render JSON controller action result as HTML-escaped string and keep original response content type.
        /// </summary>
        public static MvcHtmlString JsonAction(this HtmlHelper htmlHelper, string action, object routeValueDict)
        {
            var parentContentType = htmlHelper.ViewContext.HttpContext.Response.ContentType;
            var actionResult = htmlHelper.Action(action, routeValueDict);
            htmlHelper.ViewContext.HttpContext.Response.ContentType = parentContentType;

            return actionResult;
        }

        /// <summary>
        /// Render JSON controller action result as HTML-escaped string and keep original response content type.
        /// </summary>
        public static MvcHtmlString JsonAction(this HtmlHelper htmlHelper, string action, string controller, object routeValueDict)
        {
            var parentContentType = htmlHelper.ViewContext.HttpContext.Response.ContentType;
            var actionResult = htmlHelper.Action(action, controller, routeValueDict);
            htmlHelper.ViewContext.HttpContext.Response.ContentType = parentContentType;

            return actionResult;
        }

        /// <summary>
        /// Render JSON controller action result as HTML-escaped string and keep original response content type.
        /// </summary>
        public static MvcHtmlString JsonAction(this HtmlHelper htmlHelper, string action, string controller)
        {
            var parentContentType = htmlHelper.ViewContext.HttpContext.Response.ContentType;
            var actionResult = htmlHelper.Action(action, controller);
            htmlHelper.ViewContext.HttpContext.Response.ContentType = parentContentType;

            return actionResult;
        }
    }
}
