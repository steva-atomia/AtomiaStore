
namespace Atomia.Store.Core
{
    /// <summary>
    /// Interface for functionality to get localized resource strings based on resource name keys.
    /// </summary>
    public interface IResourceProvider
    {
        /// <summary>
        /// Get resource string from resource name
        /// </summary>
        /// <param name="resourceName">The key to get the resource string from.</param>
        /// <returns>The localized resource string.</returns>
        string GetResource(string resourceName);
    }
}
