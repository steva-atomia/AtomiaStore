using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Models
{
    public class RenewalPeriodInput: RenewalPeriod
    {
        public override int Period { get; set; }

        public override string Unit { get; set; }
    }
}
