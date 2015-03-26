
namespace Atomia.Store.Core
{
    /// <summary>
    /// Representation of a country
    /// </summary>
    public sealed class Country
    {
        /// <summary>
        /// The name of the country
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ISO 3166-1 alpha-2
        /// </summary>
        /// <remarks>
        /// Should use exceptions for Greece (EL instead of GR) and Great Britain (GB instead of UK), as used by European Commission for VAT purposes.
        /// </remarks>
        public string Code { get; set; }
    }
}
