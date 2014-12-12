using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public sealed class CustomAttribute
    {
        public string Name { get; set; }

        public List<string> Values { get; set; }

        public bool RequiredInput { get; set; }
    }
}
