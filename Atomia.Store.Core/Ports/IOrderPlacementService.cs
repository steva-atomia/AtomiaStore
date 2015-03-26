
namespace Atomia.Store.Core
{
    /// <summary>
    /// Interface for service that can place orders in the billing system.
    /// </summary>
    public interface IOrderPlacementService
    {
        /// <summary>
        /// Place an order in the billing system.
        /// </summary>
        /// <param name="orderContext">The collected order data to use for placing the order. <see cref="OrderContext"/></param>
        /// <returns>The results from placing the order. <see cref="OrderResult"/></returns>
        OrderResult PlaceOrder(OrderContext orderContext);
    }
}
