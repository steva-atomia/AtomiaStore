using System;
using System.Web;
using System.Web.Mvc;
using Atomia.OrderPage.UI.Infrastructure;

namespace Atomia.OrderPage.ActionTrail 
{
    public class LogErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            
            if (filterContext.IsChildAction || filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            var ex = filterContext.Exception;
            var httpException = ex as HttpException;

            var logger = new Logger();
            logger.LogException(filterContext.Exception, string.Format("Caught unhandled exception in Order Page v2.\r\n {0}", ex.Message + "\r\n" + ex.StackTrace));

            if (httpException != null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    switch (httpException.GetHttpCode())
                    {
                        case 401:
                            filterContext.Result = JsonEnvelope.Error("Not enough privileges.");
                            break;
                        case 404:
                            filterContext.Result = JsonEnvelope.Error("Resource not found.");
                            break;
                        default:
                            filterContext.Result = JsonEnvelope.Error("An unknown error has occurred.");
                            break;
                    }
                }
                else
                {
                    switch (httpException.GetHttpCode())
                    {
                        case 401:
                            filterContext.Result = new ViewResult { ViewName = "~/Views/Error/NotEnoughPrivileges.cshtml" };
                            break;
                        case 404:
                            filterContext.Result = new ViewResult { ViewName = "~/Views/Error/NotFound.cshtml" };
                            break;
                        default:
                            filterContext.Result = new ViewResult { ViewName = "~/Views/Error/Unknown.cshtml" };
                            break;
                    }
                }

                filterContext.HttpContext.Response.StatusCode = httpException.GetHttpCode();
            }
            else
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = JsonEnvelope.Error("An unknown error has occurred.");
                }
                else 
                {
                    filterContext.Result = new ViewResult { ViewName = "~/Views/Error/Unknown.cshtml" };
                }
                
                filterContext.HttpContext.Response.StatusCode = 500;
            }

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}
