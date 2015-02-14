using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    public class ResellerProvider : IResellerProvider
    {
        private readonly IResellerDataProvider resellerDataProvider;

        public ResellerProvider(IResellerDataProvider resellerDataProvider)
        {
            if (resellerDataProvider == null)
            {
                throw new ArgumentNullException("resellerDataProvider");
            }

            this.resellerDataProvider = resellerDataProvider;
        }

        public Reseller GetReseller()
        {
            var resellerData =  resellerDataProvider.GetResellerAccountData();
            var defaultResellerData = resellerDataProvider.GetDefaultResellerAccountData();

            var reseller = new Reseller
            {
                Id = resellerData.Id,
                IsSubReseller = resellerData.Id != defaultResellerData.Id
            };

            return reseller;
        }
    }
}
