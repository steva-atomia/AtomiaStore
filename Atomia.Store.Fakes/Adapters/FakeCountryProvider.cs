using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;

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
