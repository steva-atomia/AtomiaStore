using Atomia.Store.Core;
using System;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakeResellerProvider : IResellerProvider
    {
        public Reseller GetReseller()
        {
            return new Reseller
            {
                Id = new Guid("B77B8B91-741B-4CF1-88B4-FEB21550055C"),
                IsSubReseller = false
            };
        }
    }
}
