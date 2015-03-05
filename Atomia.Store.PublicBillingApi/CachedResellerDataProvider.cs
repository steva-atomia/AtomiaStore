using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Web;

namespace Atomia.Store.PublicBillingApi
{
    public sealed class CachedResellerDataProvider : IResellerDataProvider
    {
        private readonly IResellerDataProvider backingProvider;
        private readonly IResellerIdentifierProvider resellerIdentifierProvider;

        public CachedResellerDataProvider(IResellerDataProvider resellerDataProvider, IResellerIdentifierProvider resellerIdentifierProvider)
        {
            if (resellerDataProvider == null)
            {
                throw new ArgumentException("resellerDataProvider");
            }

            if (resellerIdentifierProvider == null)
            {
                throw new ArgumentNullException("resellerIdentifierProvider");
            }

            this.backingProvider = resellerDataProvider;
            this.resellerIdentifierProvider = resellerIdentifierProvider;
        }

        public AccountData GetResellerAccountData()
        {
            AccountData resellerData = null;
            var resellerIdentifier = resellerIdentifierProvider.GetResellerIdentifier();
            var cacheKey = "default";

            if (resellerIdentifier != null && !string.IsNullOrEmpty(resellerIdentifier.AccountHash))
            {
                cacheKey = resellerIdentifier.AccountHash;
            }
            else if (resellerIdentifier != null && !string.IsNullOrEmpty(resellerIdentifier.BaseUrl))
            {
                cacheKey = resellerIdentifier.BaseUrl;
            }

            if (!TryGetCachedData(cacheKey, out resellerData))
            {
                resellerData = backingProvider.GetResellerAccountData();
                SetCachedData(cacheKey, resellerData);
            }

            return resellerData;
        }

        public AccountData GetDefaultResellerAccountData()
        {
            AccountData resellerData = null;

            if (!TryGetCachedData("default", out resellerData))
            {
                resellerData = backingProvider.GetDefaultResellerAccountData();
                SetCachedData("default", resellerData);
            }

            return resellerData;
        }

        private bool TryGetCachedData(string key, out AccountData accountData)
        {
            accountData = HttpContext.Current.Cache[key] as AccountData;

            return accountData != null;
        }

        private void SetCachedData(string key, AccountData accountData)
        {
            HttpContext.Current.Cache[key] = accountData;
        }
    }
}
