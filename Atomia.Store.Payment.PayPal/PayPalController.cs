using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using Atomia.Common;
using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using Atomia.Store.AspNetMvc.Ports;

namespace Atomia.Store.Payment.PayPal
{
    public class PayPalController : Controller
    {
        private readonly AtomiaBillingPublicService billingApi = DependencyResolver.Current.GetService<AtomiaBillingPublicService>();
        private readonly PaymentUrlProvider urlProvider = DependencyResolver.Current.GetService<PaymentUrlProvider>();

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Confirm()
        {
            string token = this.Request.Params["token"];
            string PayerID = this.Request.Params["PayerID"];

            var transaction = billingApi.GetPaymentTransactionById(token);

            if (transaction == null)
            {
                throw new ArgumentException("Invalid token");
            }

            var cancelUrl = transaction.Attributes.Any(item => item.Name == "CancelUrl")
                ? transaction.Attributes.First(item => item.Name == "CancelUrl").Value
                : urlProvider.CancelUrl;

            var model = new PayPalViewModel
            {
                PayAmount = transaction.Amount.ToString(".00"),
                ReferenceNumber = token,
                PayerId = PayerID,
                CurrencyFormat = CultureHelper.CURRENCY_FORMAT,
                NumberFormat = CultureHelper.NUMBER_FORMAT,
                Currency = transaction.CurrencyCode,
                CancelUrl = cancelUrl
            };

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Confirm(string token, string PayerID)
        {
            var transaction = billingApi.GetPaymentTransactionById(token);

            if (transaction == null)
            {
                throw new ArgumentException("Invalid token");
            }

            var attributeDatas = transaction.Attributes.ToList();
            if (!attributeDatas.Any(item => item.Name == "token"))
            {
                attributeDatas.Add(new AttributeData { Name = "token", Value = token });
            }
            else
            {
                attributeDatas.First(item => item.Name == "token").Value = token;
            }

            if (!attributeDatas.Any(item => item.Name == "payerid"))
            {
                attributeDatas.Add(new AttributeData { Name = "payerid", Value = PayerID });
            }
            else
            {
                attributeDatas.First(item => item.Name == "payerid").Value = PayerID;
            }

            var nameValues = new List<NameValue>();

            foreach (var item in attributeDatas)
            {
                nameValues.Add(new NameValue { Name = item.Name, Value = item.Value });
            }

            var finishedTransaction = billingApi.FinishPayment(transaction.TransactionId);

            if (finishedTransaction == null)
            {
                // error: transaction does not exist
                throw new ArgumentException("Transcation could not be finished.");
            }

            var locale = CultureInfo.CreateSpecificCulture("en-US");

            // we send it as a string to avoid culture issues
            var amountStr = transaction.Amount.ToString(locale);

            return RedirectToAction("Index", "Checkout", new {
                amount = amountStr,
                transactionReference = finishedTransaction.TransactionReference,
                transactionReferenceType = 0,
                status = finishedTransaction.Status
            });
        }
    }
}
