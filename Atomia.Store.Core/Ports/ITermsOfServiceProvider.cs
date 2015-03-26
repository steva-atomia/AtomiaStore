using System.Collections.Generic;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Interface for provising <see cref="TermsOfService"/>
    /// </summary>
    public interface ITermsOfServiceProvider
    {
        /// <summary>
        /// Get all applicable <see cref="TermsOfService"/> for the current customer.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TermsOfService> GetTermsOfService();

        /// <summary>
        /// Get <see cref="TermsOfService"/> based on specified id.
        /// </summary>
        /// <param name="id">The id for which to get the <see cref="TermsOfSerivce"/></param>
        /// <returns>The found <see cref="TermsOfService"/></returns>
        /// <exception cref="System.ArgumentException">Should throw <see cref="ArgumentException"/> if no matching <see cref="TermsOfService"/> are found.</exception>
        TermsOfService GetTermsOfService(string id);
    }
}
