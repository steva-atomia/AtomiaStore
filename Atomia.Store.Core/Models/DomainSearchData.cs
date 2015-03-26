using System.Collections.Generic;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Wrapper to collection of <see cref="DomainResult"/>
    /// </summary>
    public sealed class DomainSearchData
    {
        /// <summary>
        /// The results
        /// </summary>
        public IEnumerable<DomainResult> Results { get; set; }

        /// <summary>
        /// Identifier of this search.
        /// </summary>
        public int DomainSearchId { get; set; }

        /// <summary>
        /// If this search is finished or not, i.e. all results have received their final status.
        /// </summary>
        public bool FinishSearch { get; set; }
    }
}
