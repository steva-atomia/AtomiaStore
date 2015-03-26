
namespace Atomia.Store.Core
{
    /// <summary>
    /// Customer type representation
    /// </summary>
    public sealed class CustomerType
    {
        /// <summary>
        /// Human readable name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Id that should be unique for each customer type.
        /// </summary>
        public string Id { get; set; }
    }
}
