
namespace Atomia.Store.Core
{
    /// <summary>
    /// Terms of service, or terms and conditions, for ordering a <see cref="Product"/>
    /// </summary>
    public sealed class TermsOfService
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Human readable name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The actual text of the terms.
        /// </summary>
        public string Terms { get; set; }
    }
}
