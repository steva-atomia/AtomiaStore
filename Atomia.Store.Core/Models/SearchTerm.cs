
namespace Atomia.Store.Core
{
    /// <summary>
    /// General representation of a search term
    /// </summary>
    public sealed class SearchTerm
    {
        /// <summary>
        /// SearchTerm constructor
        /// </summary>
        public SearchTerm(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        /// <summary>
        /// Search term default constructor
        /// </summary>
        public SearchTerm()
        {

        }

        /// <summary>
        /// The type of data the <see cref="Value"/> refers to.
        /// </summary>
        /// <example>"category"</example>
        public string Key { get; set; }

        /// <summary>
        /// The specific term to search for.
        /// </summary>
        /// <example>"HostingPackage"</example>
        public string Value { get; set; }
    }
}
