using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.AspNetMvc.Ports;
using System.Web.Mvc;
using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Helpers
{
    public class PaymentMethodGuiPluginsProvider
    {
        private readonly IEnumerable<PaymentMethodGuiPlugin> paymentMethodPlugins = DependencyResolver.Current.GetServices<PaymentMethodGuiPlugin>();
        private readonly IPaymentMethodsProvider paymentMethodsProvider = DependencyResolver.Current.GetService<IPaymentMethodsProvider>();

        private IEnumerable<PaymentMethodGuiPlugin> availablePlugins;

        public IEnumerable<PaymentMethodGuiPlugin> AvailablePaymentMethodGuiPlugins
        {
            get
            {
                if (this.availablePlugins != null)
                {
                    return this.availablePlugins;
                }

                var paymentMethods = paymentMethodsProvider.GetPaymentMethods();
                var availablePaymentMethodPlugins = new List<PaymentMethodGuiPlugin>();

                foreach (var plugin in paymentMethodPlugins)
                {
                    if (paymentMethods.Any(method => method.Id == plugin.Id))
                    {
                        availablePaymentMethodPlugins.Add(plugin);
                    }
                }

                this.availablePlugins = availablePaymentMethodPlugins.ToList();

                return this.availablePlugins;
            }
        }

        public string DefaultPaymentMethodId
        {
            get
            {
                var preferredPaymentMethodId = paymentMethodsProvider.GetDefaultPaymentMethod().Id;
                var defaultPaymentMethodId = preferredPaymentMethodId;

                var activePaymentMethodIds = AvailablePaymentMethodGuiPlugins.Select(p => p.Id);

                if (!activePaymentMethodIds.Contains(preferredPaymentMethodId))
                {
                    defaultPaymentMethodId = activePaymentMethodIds.First();
                }

                return defaultPaymentMethodId;
            }
        }
    }
}
