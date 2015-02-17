using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.AspNetMvc.Ports
{
    public abstract class PaymentMethodForm
    {
        public abstract string Id { get; }

        public virtual string PartialViewName { get { return "_" + this.Id; } }
    }
}
