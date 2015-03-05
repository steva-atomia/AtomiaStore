
namespace Atomia.Store.Core
{
    public interface IOrderPlacementService
    {
        OrderResult PlaceOrder(OrderContext orderContext);
    }
}
