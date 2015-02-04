using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public interface ICountryProvider
    {
        Country GetDefaultCountry();

        IEnumerable<Country> GetCountries();
    }
}
