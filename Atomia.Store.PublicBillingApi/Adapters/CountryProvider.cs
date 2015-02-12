using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    public class CountryProvider : PublicBillingApiClient, ICountryProvider
    {
        private readonly AccountData resellerData;

        public CountryProvider(ResellerDataProvider resellerDataProvider, PublicBillingApiProxy billingApi)
            : base(billingApi)
        {
            if (resellerDataProvider == null)
            {
                throw new ArgumentNullException("resellerDataProvider");
            }

            resellerData = resellerDataProvider.GetResellerAccountData();
        }

        public Core.Country GetDefaultCountry()
        {
            return new Core.Country
            {
                Code = resellerData.DefaultCountry.Code,
                Name = resellerData.DefaultCountry.Name
            };
        }

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
