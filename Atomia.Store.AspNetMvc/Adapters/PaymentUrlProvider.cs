using Atomia.Store.AspNetMvc.Helpers;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Atomia.Store.AspNetMvc.Adapters
{
    /// <summary>
    /// <see cref="Atomia.Store.Core.PaymentUrlProvider"/> for actions in <see cref="Atomia.Store.AspNetMvc.Controller.CheckoutController"/>
    /// </summary>
    public sealed class PaymentUrlProvider : Atomia.Store.Core.PaymentUrlProvider
    {
        /// <inheritdoc/>
        public override string SuccessUrl
        {
            get
            {
                return QualifiedUrl("Success", "Checkout");
            }
        }

        /// <inheritdoc/>
        public override string FailureUrl
        {
            get
            {
                return QualifiedUrl("Failure", "Checkout");
            }
        }

        /// <inheritdoc/>
        public override string CancelUrl
        {
            get
            {
                return QualifiedUrl("Index", "Checkout");
            }
        }

        /// <inheritdoc/>
        public override string DefaultPaymentUrl
        {
            get
            {
                return QualifiedUrl("Payment", "Checkout");
            }
        }

        /// <inheritdoc/>
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
