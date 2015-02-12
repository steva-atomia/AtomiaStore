using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.PublicBillingApi
{
    public class ResellerDataProvider : PublicBillingApiClient
    {
        private readonly IResellerIdentifierProvider resellerIdentifierProvider;

        public ResellerDataProvider(IResellerIdentifierProvider resellerIdentifierProvider, PublicBillingApiProxy billingApi)
            : base(billingApi)
        {
            if (resellerIdentifierProvider == null)
            {
                throw new ArgumentNullException("resellerIdentifierProvider");
            }

            this.resellerIdentifierProvider = resellerIdentifierProvider;
        }

        public AccountData GetResellerAccountData()
        {
            var resellerIdentifier = resellerIdentifierProvider.GetResellerIdentifier();

            if (resellerIdentifier != null && !string.IsNullOrEmpty(resellerIdentifier.AccountHash))
            {
                return BillingApi.GetAccountDataByHash(resellerIdentifier.AccountHash);
            }
            else if (resellerIdentifier != null && !string.IsNullOrEmpty(resellerIdentifier.BaseUrl))
            {
                return BillingApi.GetResellerDataByUrl(resellerIdentifier.BaseUrl);
            }

            return BillingApi.GetDefaultResellerData();
        }
    }
}
