
namespace Atomia.Store.Core
{
    /// <summary>
    /// Interface for getting the current <see cref="Reseller"/>
    /// </summary>
    public interface IResellerProvider
    {
        /// <summary>
        /// Get the current <see cref="Reseller"/>
        /// </summary>
        /// <returns>The current <see cref="Reseller"/></returns>
        Reseller GetReseller();
    }
}
