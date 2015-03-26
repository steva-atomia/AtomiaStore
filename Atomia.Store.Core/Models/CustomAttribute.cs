
namespace Atomia.Store.Core
{
    /// <summary>
    /// A custom attribute to add to other classes
    /// </summary>
    /// <remarks>E.g. used by <see cref="CartItem"/>, <see cref="DomainResult"/> and <see cref="Product"/>.</remarks>
    public sealed class CustomAttribute
    {
        /// <summary>
        /// The name (key) of the custom attribute
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The value of the custom attribute.
        /// </summary>
        public string Value { get; set; }
    }
}
