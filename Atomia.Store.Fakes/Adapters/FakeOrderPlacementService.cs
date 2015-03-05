using Atomia.Store.Core;
using System;

namespace Atomia.Store.Fakes.Adapters
{
    public sealed class FakeOrderPlacementService : IOrderPlacementService
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

        public OrderResult PlaceOrder(OrderContext orderContext)
        {
            return new OrderResult { RedirectUrl = urlProvider.SuccessUrl };
        }
    }
}
