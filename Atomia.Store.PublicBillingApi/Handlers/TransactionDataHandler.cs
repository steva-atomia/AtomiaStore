using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.PublicBillingApi.Handlers
{
    /// <summary>
    /// Base for handlers that amend Atomia Billing Public Service payment transaction with collected order data.
    /// </summary>
    public abstract class TransactionDataHandler
    {
        /// <summary>
        /// Amend payment transaction with order data.
        /// </summary>
        /// <param name="paymentTransaction">The payment transaction to amend</param>
        /// <param name="orderContext">The order data</param>
        /// <returns>The amended transaction</returns>
        public abstract PublicPaymentTransaction AmendPaymentTransaction(PublicPaymentTransaction paymentTransaction, PublicOrderContext orderContext);
    }
}
