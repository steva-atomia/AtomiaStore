using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    /// <summary>
    /// <see cref="Atomia.Store.Core.ICountryProvider"/> to get default and available countries for reseller in Atomia Billing.
    /// </summary>
    public sealed class CountryProvider : PublicBillingApiClient, ICountryProvider
    {
        private readonly AccountData resellerData;

        /// <summary>
        /// Construct a new instance
        /// </summary>
        public CountryProvider(IResellerDataProvider resellerDataProvider, PublicBillingApiProxy billingApi)
            : base(billingApi)
        {
            if (resellerDataProvider == null)
            {
                throw new ArgumentNullException("resellerDataProvider");
            }

            resellerData = resellerDataProvider.GetResellerAccountData();
        }

        /// <summary>
        /// Get current reseller's default country.
        /// </summary>
        public Core.Country GetDefaultCountry()
        {
            return new Core.Country
            {
                Code = resellerData.DefaultCountry.Code,
                Name = resellerData.DefaultCountry.Name
            };
        }

        /// <summary>
        /// Get all available countries in Atomia Biling.
        /// </summary>
        public IEnumerable<Core.Country> GetCountries()
        {
            var apiCountries = BillingApi.GetCountries();

            return apiCountries.Select(c => new Core.Country
            {
                Code = c.Code,
                Name = c.Name
            });
        }
    }
}
