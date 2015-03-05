using Atomia.Store.Core;
using System.Collections.Generic;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakeCountryProvider : ICountryProvider
    {
        public Country GetDefaultCountry()
        {
            return new Country
            {
                Name = "Sweden",
                Code = "SE"
            };
        }

        public IEnumerable<Country> GetCountries()
        {
            return new List<Country>
            {
                new Country
                {
                    Name = "Serbia",
                    Code = "RS"
                },
                new Country
                {
                    Name = "Sweden",
                    Code = "SE"
                },
                new Country
                {
                    Name = "United States",
                    Code = "US"
                }
            };
        }
    }
}
