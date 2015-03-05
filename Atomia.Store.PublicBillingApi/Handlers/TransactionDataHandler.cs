using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.PublicBillingApi.Handlers
{
    public abstract class TransactionDataHandler
    {
        public virtual PublicPaymentTransaction AmendPaymentTransaction(PublicPaymentTransaction paymentTransaction, PublicOrderContext orderContext)
        {
            return paymentTransaction;
        }
    }
}
