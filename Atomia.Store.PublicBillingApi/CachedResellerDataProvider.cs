using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Web;

namespace Atomia.Store.PublicBillingApi
{
    /// <summary>
    /// Caching decorator to be used with the base <see cref="ResellerDataProvider"/>
    /// </summary>
    public sealed class CachedResellerDataProvider : IResellerDataProvider
    {
        private readonly IResellerDataProvider backingProvider;
        private readonly IResellerIdentifierProvider resellerIdentifierProvider;

        /// <summary>
        /// Create new instance that wraps base provider
        /// </summary>
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

        /// <summary>
        /// Get current reseller account data from cache or get and cache data form base provider.
        /// </summary>
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

        /// <summary>
        /// Get default reseller account data from cache or get and cache data form base provider.
        /// </summary>
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
