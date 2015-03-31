using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    /// <summary>
    /// Provides default and avaialble languages for current reseller from Atomia Billing.
    /// </summary>
    public sealed class LanguageProvider : PublicBillingApiClient, ILanguageProvider
    {
        private readonly AccountData resellerData;
        private readonly IResourceProvider resourceProvider;

        /// <summary>
        /// Construct new instance with current reselller
        /// </summary>
        public LanguageProvider(IResourceProvider resourceProvider, IResellerDataProvider resellerDataProvider, PublicBillingApiProxy billingApi)
            : base(billingApi)
        {
            if (resourceProvider == null)
            {
                throw new ArgumentNullException("resourceProvider");
            }

            if (resellerDataProvider == null)
            {
                throw new ArgumentNullException("resellerDataProvider");
            }

            this.resourceProvider = resourceProvider;
            this.resellerData = resellerDataProvider.GetResellerAccountData();
        }

        /// <summary>
        /// Get available languages for current reseller.
        /// </summary>
        public IList<Language> GetAvailableLanguages()
        {
            var languages = resellerData.Languages.Select(lang => Language.CreateLanguage(resourceProvider, lang)).ToList();

            return languages;
        }

        /// <summary>
        /// Get default language for current reseller. Fallback to "en-us" if reseller is not available or has no default language.
        /// </summary>
        public Language GetDefaultLanguage()
        {
            Language language;

            if (resellerData != null && !String.IsNullOrEmpty(resellerData.DefaultLanguage))
            {
                language = Language.CreateLanguage(resourceProvider, resellerData.DefaultLanguage);
            }
            else
            {
                language = Language.CreateLanguage(resourceProvider, "en-us");
            }
            
            return language;
        }
    }
}
