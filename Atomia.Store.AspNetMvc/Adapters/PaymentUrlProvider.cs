using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Atomia.Store.Core;


namespace Atomia.Store.AspNetMvc.Adapters
{
    public class PaymentUrlProvider : Atomia.Store.Core.PaymentUrlProvider
    {
        public override string SuccessUrl
        {
            get
            {
                return QualifiedUrl("Success", "Checkout");
            }
        }

        public override string FailureUrl
        {
            get
            {
                return QualifiedUrl("Failure", "Checkout");
            }
        }

        public override string CancelUrl
        {
            get
            {
                return QualifiedUrl("Index", "Checkout");
            }
        }

        public override string PaymentRedirectUrl
        {
            get
            {
                return QualifiedUrl("Payment", "Checkout");
            }
        }

        public override string QualifiedUrl(string path)
        {
            var currentUrl = HttpContext.Current.Request.Url;
            var baseUri = new Uri(string.Format("{0}://{1}/", currentUrl.Scheme, currentUrl.Authority));
            var qualifiedUri = new Uri(baseUri, path);

            return qualifiedUri.ToString();
        }

        private string QualifiedUrl(string action, string controller)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext, RouteTable.Routes);
            var path = urlHelper.Action(action, controller);

            return QualifiedUrl(path);
        }
    }
}
