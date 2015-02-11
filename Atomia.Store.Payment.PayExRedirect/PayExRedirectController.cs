using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using Atomia.Store.Core;
using System.Globalization;
using Atomia.Store.AspNetMvc.Ports;

namespace Atomia.Store.Payment.PayExRedirect
{
    public class PayExRedirectController : Controller
    {
        // TODO: Better way of instantiating this?
        private readonly AtomiaBillingPublicService billingPublicService = new AtomiaBillingPublicService();
        private readonly PaymentUrlProvider urlProvider = DependencyResolver.Current.GetService<PaymentUrlProvider>();

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Confirm(string orderRef)
        {
            var transaction = billingPublicService.GetPaymentTransactionById(orderRef);
            
            if (transaction == null)
            {
                // error: transaction does not exist
                throw new ArgumentException("Token is invalid");
            }

            List<AttributeData> attributeDatas = transaction.Attributes.ToList();
            if (!attributeDatas.Any(item => item.Name == "orderRef"))
            {
                attributeDatas.Add(new AttributeData { Name = "orderRef", Value = orderRef });
            }
            else
            {
                attributeDatas.First(item => item.Name == "orderRef").Value = orderRef;
            }

            var finishedTransaction = billingPublicService.FinishPayment(transaction.TransactionId);;

            if (finishedTransaction == null)
            {
                // error: transaction does not exist
                throw new ArgumentException("Transcation could not be finished.");
            }

            CultureInfo locale = CultureInfo.CreateSpecificCulture("en-US");

            // we send it as a string to avoid culture issues
            string amountStr = transaction.Amount.ToString(locale);

            return RedirectToAction("Index", "Checkout", new {
                amount = amountStr,
                transactionReference = finishedTransaction.TransactionReference,
                transactionReferenceType = 0,
                status = finishedTransaction.Status
            });
        }
    }
}
