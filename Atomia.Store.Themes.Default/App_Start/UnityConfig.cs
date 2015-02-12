using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi.Ports;
using Atomia.Store.PublicBillingApi;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Web.Mvc;
using Unity.Mvc5;


namespace Atomia.Store.Themes.Default
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // Core types
            container.RegisterType<IViewEngine, Atomia.Store.AspNetMvc.Infrastructure.RazorThemeViewEngine>("RazorThemeViewEngine");
            container.RegisterType<IModelBinderProvider, Atomia.Store.AspNetMvc.Infrastructure.ModelBinderProvider>();
            container.RegisterType<ILogger, Atomia.Store.ActionTrail.Adapters.Logger>();
            container.RegisterType<ICartProvider, Atomia.Store.AspNetMvc.Adapters.CartProvider>();
            container.RegisterType<IContactDataProvider, Atomia.Store.AspNetMvc.Adapters.ContactDataProvider>();
            container.RegisterType<ICurrencyProvider, Atomia.Store.AspNetMvc.Adapters.CurrencyProvider>();
            container.RegisterType<ICurrencyFormatter, Atomia.Store.AspNetMvc.Adapters.CurrencyFormatter>();
            container.RegisterType<IResourceProvider, Atomia.Store.WebBase.Adapters.ResourceProvider>();
            container.RegisterType<ICustomerTypeProvider, Atomia.Store.AspNetMvc.Adapters.CustomerTypeProvider>();
            container.RegisterType<ITermsOfServiceProvider, Atomia.Store.AspNetMvc.Adapters.TermsOfServiceProvider>();
            container.RegisterType<PaymentUrlProvider, Atomia.Store.AspNetMvc.Adapters.PaymentUrlProvider>();
            container.RegisterType<IThemeNamesProvider, Atomia.Store.AspNetMvc.Adapters.ThemeNamesProvider>();
            

            // Payment plugins
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.AdyenHpp.AdyenHppGuiPlugin>("AdyenHpp");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.DibsFlexwin.DibsFlexwinGuiPlugin>("DibsFlexwin");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.Invoice.InvoiceByEmailGuiPlugin>("InvoiceByEmail");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.Invoice.InvoiceByPostGuiPlugin>("InvoiceByPost");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.PayExRedirect.PayExRedirectGuiPlugin>("PayExRedirect");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.PayPal.PayPalGuiPlugin>("PayPal");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.WorldPay.WorldPayGuiPlugin>("WorldPay");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.WorldPayXml.WorldPayXmlGuiPlugin>("WorldPayXml");

            container.RegisterType<PaymentMethodHandler, Atomia.Store.Payment.AdyenHpp.AdyenHppHandler>("AdyenHpp");
            container.RegisterType<PaymentMethodHandler, Atomia.Store.Payment.DibsFlexwin.DibsFlexwinHandler>("DibsFlexwin");
            container.RegisterType<PaymentMethodHandler, Atomia.Store.Payment.Invoice.InvoiceByEmailHandler>("InvoiceByEmail");
            container.RegisterType<PaymentMethodHandler, Atomia.Store.Payment.Invoice.InvoiceByPostHandler>("InvoiceByPost");
            container.RegisterType<PaymentMethodHandler, Atomia.Store.Payment.PayExRedirect.PayExRedirectHandler>("PayExRedirect");
            container.RegisterType<PaymentMethodHandler, Atomia.Store.Payment.PayPal.PayPalHandler>("PayPal");
            container.RegisterType<PaymentMethodHandler, Atomia.Store.Payment.WorldPay.WorldPayHandler>("WorldPay");
            container.RegisterType<PaymentMethodHandler, Atomia.Store.Payment.WorldPayXml.WorldPayXmlHandler>("WorldPayXml");


            // PublicBillingApi types
            container.RegisterType<PublicBillingApiClient, PublicBillingApiClient>();
            container.RegisterType<PublicBillingApiProxy, PublicBillingApiProxy>();
            container.RegisterType<ResellerDataProvider, ResellerDataProvider>();


            // ViewModels
            container.RegisterType<DomainViewModel, DomainViewModel>();
            container.RegisterType<ProductListingViewModel, ProductListingViewModel>();
            container.RegisterType<ProductListingDataModel, ProductListingDataModel>();
            container.RegisterType<AccountViewModel, DefaultAccountViewModel>();
            container.RegisterType<CheckoutViewModel, DefaultCheckoutViewModel>();


            // Fakes
            container.RegisterType<ICartPricingService, Atomia.Store.Fakes.Adapters.FakePricingProvider>();
            container.RegisterType<IItemPresenter, Atomia.Store.Fakes.Adapters.FakeItemPresenter>();
            container.RegisterType<ILanguageProvider, Atomia.Store.Fakes.Adapters.FakeLanguageProvider>();
            container.RegisterType<IResellerProvider, Atomia.Store.Fakes.Adapters.FakeResellerProvider>();
            container.RegisterType<ICountryProvider, Atomia.Store.Fakes.Adapters.FakeCountryProvider>();
            container.RegisterType<IPaymentMethodsProvider, Atomia.Store.Fakes.Adapters.FakePaymentMethodsProvider>();
            container.RegisterType<IProductListProvider, Atomia.Store.Fakes.Adapters.FakeCategoryProductsProvider>();
            container.RegisterType<IDomainsProvider, Atomia.Store.Fakes.Adapters.FakePremiumDomainSearchProvider>();
            container.RegisterType<IOrderPlacementService, Atomia.Store.Fakes.Adapters.FakeOrderPlacementService>();
            container.RegisterType<IResellerIdentifierProvider, Atomia.Store.Fakes.Adapters.FakeRootResellerIdentifierProvider>();

            container.LoadConfiguration();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
