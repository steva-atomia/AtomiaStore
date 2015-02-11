using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakeResellerProvider : IResellerProvider
    {
        public Guid GetResellerId()
        {
            return new Guid("B77B8B91-741B-4CF1-88B4-FEB21550055C");
        }

        public bool IsSubReseller()
        {
            return false;
        }
    }
}
