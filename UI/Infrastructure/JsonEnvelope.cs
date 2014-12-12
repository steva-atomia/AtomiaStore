using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Infrastructure
{
    /// <summary>
    /// Factory of JSON responses that correspond to the JSend spec.
    /// Always allows GET, since the response is wrapped in an object, avoiding JSON array response vulnerability.
    /// </summary>
    /// <remarks>http://labs.omniti.com/labs/jsend</remarks>
    public static class JsonEnvelope
    {
        /// <summary>
        /// JSend spec: "All went well, and (usually) some data was returned."
        /// </summary>
        public static JsonResult Success(object data)
        {
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    status = "success",
                    data = data
                }
            };
        }

        /// <summary>
        /// JSend spec: "There was a problem with the data submitted, or some pre-condition of the API call wasn't satisfied"
        /// </summary>
        public static JsonResult Fail(object data)
        {
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    status = "fail",
                    data = data
                }
            };
        }

        /// <summary>
        /// JSend spec: "There was a problem with the data submitted, or some pre-condition of the API call wasn't satisfied"
        /// Creates message data based on validation errors.
        /// </summary>
        public static JsonResult Fail(ModelStateDictionary modelState)
        {
            var invalidProperties = modelState.Where(kv => kv.Value.Errors.Count > 0);
            var data = new Dictionary<string, string>();

            foreach(var invalidProperty in invalidProperties)
            {
                var message = string.Join(",", invalidProperty.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                data.Add(invalidProperty.Key, message);
            }

            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    status = "fail",
                    data = data
                }
            };
        }

        /// <summary>
        /// JSend spec: "An error occurred in processing the request, i.e. an exception was thrown"
        /// </summary>
        public static JsonResult Error(string message)
        {
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    status = "error",
                    message = message
                }
            };
        }
    }
}
