using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Atomia.Store.UI.HtmlHelpers
{
    public static class JsonActionHelper
    {
        public static MvcHtmlString JsonAction(this HtmlHelper htmlHelper, string action, object routeValueDict)
        {
            var parentContentType = htmlHelper.ViewContext.HttpContext.Response.ContentType;
            var actionResult = htmlHelper.Action(action, routeValueDict);
            htmlHelper.ViewContext.HttpContext.Response.ContentType = parentContentType;
            return actionResult;
        }

        public static MvcHtmlString JsonAction(this HtmlHelper htmlHelper, string action, string controller, object routeValueDict)
        {
            var parentContentType = htmlHelper.ViewContext.HttpContext.Response.ContentType;
            var actionResult = htmlHelper.Action(action, controller, routeValueDict);
            htmlHelper.ViewContext.HttpContext.Response.ContentType = parentContentType;
            return actionResult;
        }
    }
}
