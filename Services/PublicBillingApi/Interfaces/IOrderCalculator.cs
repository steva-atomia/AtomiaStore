using System;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atomia.Store.Services.PublicBillingApi
{
    public interface IOrderCalculator
    {
        PublicOrder CalculateOrder(PublicOrder publicOrder);
    }
}
