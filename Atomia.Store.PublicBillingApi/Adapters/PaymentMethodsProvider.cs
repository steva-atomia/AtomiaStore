using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    public class PaymentMethodsProvider : PublicBillingApiClient, IPaymentMethodsProvider
    {
        private readonly AccountData resellerData;

        public PaymentMethodsProvider(IResellerDataProvider resellerDataProvider, PublicBillingApiProxy billingApi)
            : base(billingApi)
        {
            if (resellerDataProvider == null)
            {
                throw new ArgumentNullException("resellerDataProvider");
            }

            resellerData = resellerDataProvider.GetResellerAccountData();
        }

        public IEnumerable<Core.PaymentMethod> GetPaymentMethods()
        {
            var paymentMethods = resellerData.PaymentMethods.Select(p => new Core.PaymentMethod
            {
                Id = p.GuiPluginName
            });

            return paymentMethods;
        }

        public Core.PaymentMethod GetDefaultPaymentMethod()
        {
            var defaultPaymentMethod = new Core.PaymentMethod
            {
                Id = resellerData.DefaultPaymentMethod.GuiPluginName
            };

            return defaultPaymentMethod;
        }
    }
}
