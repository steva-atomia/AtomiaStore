using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.PublicBillingApi
{
    public interface IOrderCalculator
    {
        PublicOrder CalculateOrder(PublicOrder publicOrder);
    }

    public class PublicBillingApiProxy : IOrderCalculator
    {
        public PublicOrder CalculateOrder(PublicOrder publicOrder)
        {
            var api = new AtomiaBillingPublicService();

            return api.CalculateOrder(publicOrder);
        }
    }
}
