using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core
{
    public sealed class CustomAttribute
    {
        public string Name { get; set; }

        public List<string> Values { get; set; }

        public bool RequiredInput { get; set; }
    }
}
