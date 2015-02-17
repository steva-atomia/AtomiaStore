using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.PublicBillingApi.Ports;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using Atomia.Store.Core;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;

namespace Atomia.Store.Payment.PayExRedirect
{
    public class PayExRedirectHandler : PaymentDataHandler
    {
        private readonly PaymentUrlProvider urlProvider;

        public PayExRedirectHandler(PaymentUrlProvider urlProvider)
        {
            if (urlProvider == null)
            {
                throw new ArgumentNullException("urlProvider");
            }

            this.urlProvider = urlProvider;
        }

        public override string Id
        {
            get { return "PayExRedirect"; }
        }

        public override PaymentMethodEnum PaymentMethodType
        {
            get { return PaymentMethodEnum.PayByCard; }
        }

        public override void AmendTransaction(PaymentData paymentMethodData, PublicPaymentTransaction transaction, List<AttributeData> attributes)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext, RouteTable.Routes);
            var path = urlHelper.Action("Confirm", "PayExRedirect");

            transaction.ReturnUrl = urlProvider.QualifiedUrl(path);
            transaction.Attributes.First(item => item.Name == "CancelUrl").Value = urlProvider.CancelUrl;
        }
    }
}
