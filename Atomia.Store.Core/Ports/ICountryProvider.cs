using System.Collections.Generic;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Get countries
    /// </summary>
    public interface ICountryProvider
    {
        /// <summary>
        /// Gets the default <see cref="Country"/> 
        /// </summary>
        Country GetDefaultCountry();

        /// <summary>
        /// Gets all available countries, <see cref="Country"/>
        /// </summary>
        IEnumerable<Country> GetCountries();
    }
}
