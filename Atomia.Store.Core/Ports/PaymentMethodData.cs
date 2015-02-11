using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core
{
    public abstract class PaymentMethodData : IPaymentMethod
    {
        public abstract string Id { get; }

        public virtual object GetPaymentMethodData()
        {
            return null;
        }
    }
}
