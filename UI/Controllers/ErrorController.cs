using System;
using System.Net;
using System.Web.Mvc;
using Atomia.OrderPage.UI.Infrastructure;

namespace Atomia.OrderPage.UI.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult InternalServerError(Exception error)
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            
            if (Request.IsAjaxRequest())
            {
                Response.ContentType = "application/json";
                return JsonEnvelope.Error("An unknown error has occurred.");
            }
            else
            {
                Response.ContentType = "text/html";
                return View();
            }
        }

        public ActionResult NotFound(Exception error)
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            
            if (Request.IsAjaxRequest())
            {
                Response.ContentType = "application/json";
                return JsonEnvelope.Error("Resource not found.");
            }
            else
            {
                Response.ContentType = "text/html";
                return View();
            }
        }

        public ActionResult Forbidden(Exception error)
        {
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            Response.ContentType = "text/html";
            
            if (Request.IsAjaxRequest())
            {
                Response.ContentType = "application/json";
                return JsonEnvelope.Error("Not enough privileges.");
            }
            else
            {
                Response.ContentType = "text/html";
                return View();
            }
        }
    }
}
