using Atomia.Store.AspNetMvc.Helpers;
using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;
using Atomia.Web.Plugin.Validation.ValidationAttributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// Abstract view model for the /Checkout/Index page, collecting payment method selection and terms of service confirmations from customer.
    /// </summary>
    public abstract class CheckoutViewModel
    {
        /// <summary>
        /// Payment method selected by customer.
        /// </summary>
        public abstract PaymentMethodGuiPlugin SelectedPaymentMethod { get; }

        /// <summary>
        /// Terms of service confirmations needed by products in customer's cart
        /// </summary>
        public abstract IList<TermsOfServiceConfirmationModel> TermsOfService { get; set; }

        /// <summary>
        /// If card profile should be saved to billing system or not.
        /// </summary>
        public bool SaveCcInfo { get; set; }

        /// <summary>
        /// Saved card profile should be used to automatically pay invoices on expiration
        /// </summary>
        public bool AutoPay { get; set; }
    }

    /// <summary>
    /// Default implementation of <see cref="CheckoutViewModel"/>
    /// </summary>
    public class DefaultCheckoutViewModel : CheckoutViewModel
    {
        private readonly ITermsOfServiceProvider termsOfServiceProvider = DependencyResolver.Current.GetService<ITermsOfServiceProvider>();
        private readonly PaymentMethodGuiPluginsProvider paymentPluginsProvider = new PaymentMethodGuiPluginsProvider();

        private IList<TermsOfServiceConfirmationModel> termsOfServiceConfirmationModels = null;

        /// <summary>
        /// Construct instance with available payment methods and pre-select default payment method.
        /// </summary>
        public DefaultCheckoutViewModel()
        {
            this.PaymentMethodGuiPlugins = paymentPluginsProvider.AvailablePaymentMethodGuiPlugins;
            this.SelectedPaymentMethodId = paymentPluginsProvider.DefaultPaymentMethodId;
        }

        /// <summary>
        /// Available payment methods as <see cref="PaymentMethodGuiPlugin">PaymentMethodGuiPlugins</see>
        /// </summary>
        public virtual IEnumerable<PaymentMethodGuiPlugin> PaymentMethodGuiPlugins { get; set; }

        /// <summary>
        /// Payment method id selected by customer.
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        public string SelectedPaymentMethodId { get; set; }

        /// <summary>
        /// <see cref="PaymentMethodGuiPlugin"/> from payment method id selected by customer.
        /// </summary>
        public override PaymentMethodGuiPlugin SelectedPaymentMethod
        {
            get { return PaymentMethodGuiPlugins.Where(x => x.Id == SelectedPaymentMethodId).FirstOrDefault();}
        }

        /// <summary>
        /// List of <see cref="TermsofServiceConfirmationModel"/> for confirming terms of service applicable to products in customer's cart.
        /// </summary>
        public override IList<TermsOfServiceConfirmationModel> TermsOfService
        {
            get
            {
                if (termsOfServiceConfirmationModels == null)
                {
                    return termsOfServiceProvider.GetTermsOfService().Select(tos =>
                        new TermsOfServiceConfirmationModel
                        {
                            Id = tos.Id,
                            Name = tos.Name,
                        }).ToList();
                }

                return termsOfServiceConfirmationModels;
            }
            set
            {
                var termsOfServiceData = termsOfServiceProvider.GetTermsOfService();
                
                foreach(var val in value)
                {
                    var data = termsOfServiceData.First(tos => tos.Id == val.Id);
                    val.Name = data.Name;
                }

                termsOfServiceConfirmationModels = value;
            }
        }

        public string PaymentMethodsWithPaymentProfileToJson()
        {
            var methods = this.PaymentMethodGuiPlugins
                .Where(p => p.SupportsPaymentProfile)
                .Select(p => p.Id);

            return JsonConvert.SerializeObject(methods);
        }
    }
}
