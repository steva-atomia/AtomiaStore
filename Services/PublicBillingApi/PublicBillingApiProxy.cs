using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Services.PublicBillingApi
{
    internal class PublicBillingApiProxy : IOrderCalculator
    {
        public PublicOrder CalculateOrder(PublicOrder publicOrder)
        {
            var api = new AtomiaBillingPublicService();

            return api.CalculateOrder(publicOrder);
        }
    }
}
