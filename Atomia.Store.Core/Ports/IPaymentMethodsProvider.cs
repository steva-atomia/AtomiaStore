using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public interface IPaymentMethodsProvider
    {
        IEnumerable<PaymentMethod> GetPaymentMethods();

        PaymentMethod GetDefaultPaymentMethod();
    }
}
