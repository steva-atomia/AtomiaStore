using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.PublicBillingApi.Ports;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Atomia.Store.Core;

namespace Atomia.Store.Payment.PayPal
{
    public class PayPalHandler : PaymentDataHandler
    {
        private readonly PaymentUrlProvider urlProvider;

        public PayPalHandler(PaymentUrlProvider urlProvider)
        {
            if (urlProvider == null)
            {
                throw new ArgumentNullException("urlProvider");
            }

            this.urlProvider = urlProvider;
        }

        public override string Id
        {
            get { return "PayPal"; }
        }

        public override PaymentMethodEnum PaymentMethodType
        {
            get { return PaymentMethodEnum.PayByCard; }
        }

        public override void AmendTransaction(PaymentData paymentMethodData, PublicPaymentTransaction transaction, List<AttributeData> attributes)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext, RouteTable.Routes);
            var path = urlHelper.Action("Confirm", "PayPal");

            transaction.ReturnUrl = urlProvider.QualifiedUrl(path);
            transaction.Attributes.First(item => item.Name == "CancelUrl").Value = urlProvider.CancelUrl;
        }
    }
}
