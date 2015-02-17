using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    public class OrderPlacementService : IOrderPlacementService
    {
        private readonly PaymentUrlProvider urlProvider;

        public OrderPlacementService(PaymentUrlProvider urlProvider)
        {
            if (urlProvider == null)
            {
                throw new ArgumentNullException("urlProvider");
            }

            this.urlProvider = urlProvider;
        }

        public string PlaceOrder(Cart cart, IContactDataCollection contactData, PaymentData paymentMethodData)
        {
            throw new NotImplementedException();
        }
    }
}
