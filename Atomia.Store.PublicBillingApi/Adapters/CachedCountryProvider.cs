using Atomia.Store.Core;
using System;
using System.Collections.Generic;
using System.Web;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    /// <summary>
    /// <see cref="Atomia.Store.Core.ICountryProvider"/> to get default and available countries for reseller in Atomia Billing.
    /// </summary>
    public sealed class CachedCountryProvider : ICountryProvider
    {
        private readonly ICountryProvider countryProvider;

        /// <summary>
        /// Construct a new instance
        /// </summary>
        public CachedCountryProvider(ICountryProvider countryProvider)
        {
            if (countryProvider == null)
            {
                throw new ArgumentNullException("countryProvider");
            }

            this.countryProvider = countryProvider;
        }

        /// <summary>
        /// Get current reseller's default country.
        /// </summary>
        public Core.Country GetDefaultCountry()
        {
            return countryProvider.GetDefaultCountry();
        }

        /// <summary>
        /// Get all available countries in Atomia Biling.
        /// </summary>
        public IEnumerable<Country> GetCountries()
        {
            var countries = HttpContext.Current.Cache["CachedCountryProvider.Countries"] as IEnumerable<Country>;

            if (countries == null)
            {
                countries = this.countryProvider.GetCountries();
                HttpContext.Current.Cache["CachedCountryProvider.Countries"] = countries;
            }

            return countries;
        }
    }
}
