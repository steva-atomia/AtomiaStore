using Atomia.Store.Core;
using System.Collections.Generic;

namespace Atomia.Store.Fakes.Adapters
{
    public sealed class FakePaymentMethodsProvider : IPaymentMethodsProvider
    {
        public IEnumerable<PaymentMethod> GetPaymentMethods()
        {
            return new List<PaymentMethod>{
                new PaymentMethod 
                {
                    Id = "PayPal"
                },
                new PaymentMethod 
                {
                    Id = "InvoiceByEmail"
                },
                new PaymentMethod 
                {
                    Id = "InvoiceByPost"
                }
            };
        }

        public PaymentMethod GetDefaultPaymentMethod()
        {
            return new PaymentMethod 
                {
                    Id = "InvoiceByEmail"
                };
        }
    }
}
