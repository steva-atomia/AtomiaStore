using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakePaymentMethodsProvider : IPaymentMethodsProvider
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
