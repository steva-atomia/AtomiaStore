using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core
{
    public interface IPaymentMethodsProvider
    {
        IEnumerable<PaymentMethod> GetPaymentMethods();

        PaymentMethod GetDefaultPaymentMethod();
    }
}
