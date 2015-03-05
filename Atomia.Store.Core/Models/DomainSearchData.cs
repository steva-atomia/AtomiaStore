using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public class DomainSearchData
    {
        public IEnumerable<DomainResult> Results { get; set; }

        public int DomainSearchId { get; set; }

        public bool FinishSearch { get; set; }
    }
}
