using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;

namespace Atomia.Store.PublicBillingApi
{
    public interface IResellerDataProvider
    {
        AccountData GetResellerAccountData();

        AccountData GetDefaultResellerAccountData();
    }

    public class ResellerDataProvider : PublicBillingApiClient, IResellerDataProvider
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
            AccountData resellerData = null;

            if (resellerIdentifier != null && !string.IsNullOrEmpty(resellerIdentifier.AccountHash))
            {
                resellerData = BillingApi.GetAccountDataByHash(resellerIdentifier.AccountHash);
            }
            else if (resellerIdentifier != null && !string.IsNullOrEmpty(resellerIdentifier.BaseUrl))
            {
                resellerData = BillingApi.GetResellerDataByUrl(resellerIdentifier.BaseUrl);
            }

            if (resellerData == null)
            {
                resellerData = BillingApi.GetDefaultResellerData();
            }

            if (resellerData == null)
            {
                throw new InvalidOperationException("Could not determine reseller AccountData.");
            }

            return resellerData;
        }

        public AccountData GetDefaultResellerAccountData()
        {
            var resellerData = BillingApi.GetDefaultResellerData();

            if (resellerData == null)
            {
                throw new InvalidOperationException("Could not get default reseller AccountData.");
            }

            return resellerData;
        }
    }
}
