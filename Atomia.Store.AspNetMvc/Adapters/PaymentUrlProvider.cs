using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Atomia.Store.Core;
using Atomia.Store.AspNetMvc.Helpers;

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

        public override string DefaultPaymentUrl
        {
            get
            {
                return QualifiedUrl("Payment", "Checkout");
            }
        }

        public override string QualifiedUrl(string path)
        {
            var baseUri = BaseUriHelper.GetBaseUri();
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
