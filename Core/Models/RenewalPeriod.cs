using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atomia.Store.Core
{
    public class RenewalPeriod
    {
        public virtual int Period { get; set; }

        public virtual string Unit { get; set; }
    }
}
