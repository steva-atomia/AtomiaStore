using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Atomia.Store.Core;
using Atomia.Web.Plugin.Validation.ValidationAttributes;
using Atomia.Store.AspNetMvc.Ports;

namespace Atomia.Store.AspNetMvc.Models
{
    public abstract class CheckoutViewModel
    {
        public abstract PaymentMethodGuiPlugin SelectedPaymentMethod { get; }
    }

    public class DefaultCheckoutViewModel : CheckoutViewModel
    {
        private readonly IPaymentMethodsProvider paymentMethodsProvider = DependencyResolver.Current.GetService<IPaymentMethodsProvider>();
        private readonly ITermsOfServiceProvider termsOfServiceProvider = DependencyResolver.Current.GetService<ITermsOfServiceProvider>();

        public DefaultCheckoutViewModel()
        {
            PaymentMethods = GetAvailablePaymentMethods();
            SelectedPaymentMethodId = paymentMethodsProvider.GetDefaultPaymentMethod().Id;
            
            var paymentMethodIds = PaymentMethods.Select(p => p.Id);
            if (!paymentMethodIds.Contains(SelectedPaymentMethodId))
            {
                SelectedPaymentMethodId = paymentMethodIds.First();
            }

            TermsOfService = termsOfServiceProvider.GetTermsOfService().Select(tos => new TermsOfServiceModel
            {
                Id = tos.Id,
                Name = tos.Name,
                Terms = tos.Terms,
                Confirmed = false
            }).ToList();
        }

        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        public string SelectedPaymentMethodId { get; set; }

        public override PaymentMethodGuiPlugin SelectedPaymentMethod
        {
            get 
            {
                return PaymentMethods.Where(x => x.Id == SelectedPaymentMethodId).FirstOrDefault();
            }
        }

        public virtual List<PaymentMethodGuiPlugin> PaymentMethods { get; set;}

        public virtual List<TermsOfServiceModel> TermsOfService { get; set; }

        private List<PaymentMethodGuiPlugin> GetAvailablePaymentMethods()
        {
            var availablePaymentMethodPlugins = new List<PaymentMethodGuiPlugin>();

            var paymentMethodPlugins = DependencyResolver.Current.GetServices<PaymentMethodGuiPlugin>();
            var paymentMethods = paymentMethodsProvider.GetPaymentMethods();

            foreach (var plugin in paymentMethodPlugins)
            {
                if (paymentMethods.Any(method => method.Id == plugin.Id))
                {
                    availablePaymentMethodPlugins.Add(plugin);
                }
            }

            return availablePaymentMethodPlugins.ToList();
        }
    }
}
