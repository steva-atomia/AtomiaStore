using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    public class ResellerProvider : PublicBillingApiClient, IResellerProvider
    {
        private readonly AccountData resellerData;

        public ResellerProvider(ResellerDataProvider resellerDataProvider, PublicBillingApiProxy billingApi)
            : base(billingApi)
        {
            if (resellerDataProvider == null)
            {
                throw new ArgumentNullException("resellerDataProvider");
            }

            resellerData = resellerDataProvider.GetResellerAccountData();
        }

        public Reseller GetReseller()
        {
            var defaultAccountData = BillingApi.GetDefaultResellerData();

            var reseller = new Reseller
            {
                Id = resellerData.Id,
                IsSubReseller = resellerData.Id != defaultAccountData.Id
            };

            return reseller;
        }
    }
}
