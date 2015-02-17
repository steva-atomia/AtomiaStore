using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Atomia.Store.Core;
using Atomia.Web.Plugin.Validation.ValidationAttributes;
using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.AspNetMvc.Helpers;

namespace Atomia.Store.AspNetMvc.Models
{
    public abstract class CheckoutViewModel
    {
        public abstract PaymentMethodGuiPlugin SelectedPaymentMethod { get; }
    }


    public class DefaultCheckoutViewModel : CheckoutViewModel
    {
        public DefaultCheckoutViewModel()
        {
            var paymentPluginsProvider = new PaymentMethodGuiPluginsProvider();
            
            this.PaymentMethodGuiPlugins = paymentPluginsProvider.AvailablePaymentMethodGuiPlugins;
            this.SelectedPaymentMethodId = paymentPluginsProvider.DefaultPaymentMethodId;
        }

        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        public string SelectedPaymentMethodId { get; set; }

        public override PaymentMethodGuiPlugin SelectedPaymentMethod
        {
            get
            {
                return PaymentMethodGuiPlugins
                    .Where(x => x.Id == SelectedPaymentMethodId)
                    .FirstOrDefault();
            }
        }

        public virtual IEnumerable<PaymentMethodGuiPlugin> PaymentMethodGuiPlugins { get; set;}
    }
}
