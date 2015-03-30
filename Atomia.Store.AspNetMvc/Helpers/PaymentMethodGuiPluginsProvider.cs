using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Helpers
{
    /// <summary>
    /// Convert available <see cref="Atomia.Store.Core.PaymentMetod">PaymentMethods</see> to <see cref="Atomia.Store.AspNetMvc.PaymentMethodGuiPlugin">PaymentMethodGuiPlugins</see> 
    /// with names, descriptions and forms.
    /// </summary>
    public sealed class PaymentMethodGuiPluginsProvider
    {
        private readonly IEnumerable<PaymentMethodGuiPlugin> paymentMethodPlugins = DependencyResolver.Current.GetServices<PaymentMethodGuiPlugin>();
        private readonly IPaymentMethodsProvider paymentMethodsProvider = DependencyResolver.Current.GetService<IPaymentMethodsProvider>();

        private IEnumerable<PaymentMethodGuiPlugin> availablePlugins;

        /// <summary>
        /// Get available payment methods as <see cref="Atomia.Store.AspNetMvc.PaymentMethodGuiPlugin">PaymentMethodGuiPlugins</see> 
        /// </summary>
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

        /// <summary>
        /// Get default payment method id. 
        /// If the default id suggested by the <see cref="Atomia.Store.Core.IPaymentMethodsProvider"/> is not available as a <see cref="Atomia.Store.AspNetMvc.PaymentMethodGuiPlugin">
        /// the first available <see cref="Atomia.Store.AspNetMvc.PaymentMethodGuiPlugin"> will be used as a fallback.
        /// </summary>
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
