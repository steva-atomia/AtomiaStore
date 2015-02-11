using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakeOrderPlacementService : IOrderPlacementService
    {
        private readonly PaymentUrlProvider urlProvider;

        public FakeOrderPlacementService(PaymentUrlProvider urlProvider)
        {
            if (urlProvider == null)
            {
                throw new ArgumentNullException("urlProvider");
            }

            this.urlProvider = urlProvider;
        }

        public string PlaceOrder(Cart cart, IContactDataCollection contactData, PaymentMethodData paymentMethodData)
        {
            return urlProvider.SuccessUrl;
        }
    }
}
