using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Services.PublicBillingApi
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
