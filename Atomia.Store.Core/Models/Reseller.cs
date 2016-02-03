using System;

namespace Atomia.Store.Core
{
    /// <summary>
    /// A reseller
    /// </summary>
    public sealed class Reseller
    {
        /// <summary>
        /// Unique id of the reseller
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// If the reseller is a reseller of another reseller or not.
        /// </summary>
        public bool IsSubReseller { get; set; }

        /// <summary>
        /// If the reseller has the TaxCalculationType set to Inclusive or not.
        /// </summary>
        public bool InclusiveTaxCalculationType { get; set;  }
    }
}
