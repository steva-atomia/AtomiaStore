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
            var token = this.Request.Params["token"];
            var payerId = this.Request.Params["PayerID"];

            var transaction = billingApi.GetPaymentTransactionById(token);
            if (transaction == null)
            {
                throw new ArgumentException("Invalid token");
            }

            // Customer clicked cancel so we should mark transaction as FAILED and finish it.
            // There is no point in showing Confirm page.
            if (string.IsNullOrEmpty(payerId))
            {
                transaction.Status = "FAILED";
                transaction.StatusCode = "Cancelled";
                transaction.StatusCodeDescription = "Cancelled on PayPal page";
                return this.FinishPayment(transaction);
            }
            
            var cancelUrl = transaction.Attributes.Any(item => item.Name == "CancelUrl")
                                   ? transaction.Attributes.First(item => item.Name == "CancelUrl").Value
                                   : urlProvider.CancelUrl;

            var model = new PayPalViewModel
            {
                PayAmount = transaction.Amount.ToString(".00"),
                ReferenceNumber = token,
                PayerId = payerId,
                CurrencyFormat = CultureHelper.CURRENCY_FORMAT,
                NumberFormat = CultureHelper.NUMBER_FORMAT,
                Currency = transaction.CurrencyCode,
                CancelUrl = cancelUrl,
                Action = ""
            };

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Confirm(PayPalViewModel model)
        {
            if (ModelState.IsValid)
            {
                var transaction = billingApi.GetPaymentTransactionById(model.ReferenceNumber);
                if (transaction == null)
                {
                    // error: transaction does not exist
                    throw new ArgumentException("Token is invalid");
                }

                // Update attributes with token and PayerID.
                var attributeDatas = transaction.Attributes.ToList();
                if (!transaction.Attributes.Any(item => item.Name == "token"))
                {
                    attributeDatas.Add(new AttributeData { Name = "token", Value = model.ReferenceNumber });
                }
                else
                {
                    attributeDatas.First(item => item.Name == "token").Value = model.ReferenceNumber;
                }

                if (!attributeDatas.Any(item => item.Name == "payerid"))
                {
                    attributeDatas.Add(new AttributeData { Name = "payerid", Value = model.PayerId });
                }
                else
                {
                    attributeDatas.First(item => item.Name == "payerid").Value = model.PayerId;
                }

                transaction.Attributes = attributeDatas.ToArray();

                if (model.Action == "cancel")
                {
                    transaction.Status = "FAILED";
                    transaction.StatusCode = "Cancelled";
                    transaction.StatusCodeDescription = "Cancelled on confirmation page";
                }

                return this.FinishPayment(transaction);
            }

            return View(model);
        }

        private ActionResult FinishPayment(PublicPaymentTransaction transaction)
        {
            billingApi.UpdatePaymentTransactionData(
                transaction.TransactionId,
                transaction.Status,
                transaction.StatusCode,
                transaction.StatusCodeDescription,
                transaction.Attributes.Select(a => new NameValue { Name = a.Name, Value = a.Value }).ToArray());

            var finishedTransaction = billingApi.FinishPayment(transaction.TransactionId);
            
            if (finishedTransaction == null)
            {
                // error: transaction does not exist
                throw new ArgumentException("Transaction could not be finished.");
            }

            var status = finishedTransaction.Status;

            if (status.ToUpper() == "OK" || status.ToUpper() == "IN_PROGRESS")
            {
                return Redirect(urlProvider.SuccessUrl);
            }

            return Redirect(urlProvider.FailureUrl);
        }
    }
}
