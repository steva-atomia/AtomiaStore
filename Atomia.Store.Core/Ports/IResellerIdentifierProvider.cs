
namespace Atomia.Store.Core
{
    /// <summary>
    /// Interface for functionality to get and set reseller base url and hash via <see cref="ResellerIdentifer"/>
    /// </summary>
    public interface IResellerIdentifierProvider
    {
        /// <summary>
        /// Get the <see cref="ResellerIdentifier"/> for the current reseller.
        /// </summary>
        /// <returns>The <see cref="ResellerIdentifier"/></returns>
        ResellerIdentifier GetResellerIdentifier();

        /// <summary>
        /// Set the <see cref="ResellerIdentifier"/> for the current reseller.
        /// </summary>
        /// <param name="identifier"></param>
        void SetResellerIdentifier(ResellerIdentifier identifier);
    }
}
