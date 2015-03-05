using System;

namespace Atomia.Store.Core
{
    public sealed class Reseller
    {
        public Guid Id { get; set; }

        public bool IsSubReseller { get; set; }
    }
}
