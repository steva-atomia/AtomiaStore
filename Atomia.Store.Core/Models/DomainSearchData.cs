using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core
{
    public class DomainSearchData
    {
        public IEnumerable<DomainResult> Results { get; set; }

        public int DomainSearchId { get; set; }

        public bool FinishSearch { get; set; }
    }
}
