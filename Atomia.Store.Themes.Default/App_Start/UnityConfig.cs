using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi;
using Atomia.Store.PublicBillingApi.Handlers;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Collections.Generic;
using System.Web.Mvc;
using Unity.Mvc5;


namespace Atomia.Store.Themes.Default
{
    public class UnityConfig
    {
        private UnityContainer container;

        public void RegisterComponents()
        {
			container = new UnityContainer();

            this.RegisterCoreAdapters();

            this.RegisterProductListingProviders();

            this.RegisterDomainsProvider();

            this.RegisterViewModels();

            this.RegisterPaymentMethods();

            this.RegisterOrderHandling();

            // Un-comment to use fake static data instead of public order api.
            // RegisterFakeAdapters();

            container.LoadConfiguration();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        /// <summary>
        /// Register core types for using Asp.Net MVC and Atomia Billing public order api.
        /// </summary>
        protected void RegisterCoreAdapters()
        {
            container.RegisterType<IViewEngine, Atomia.Store.AspNetMvc.Infrastructure.RazorThemeViewEngine>("RazorThemeViewEngine");
            container.RegisterType<IModelBinderProvider, Atomia.Store.AspNetMvc.Infrastructure.ModelBinderProvider>();
            container.RegisterType<ILogger, Atomia.Store.ActionTrail.Adapters.Logger>();
            container.RegisterType<ICartProvider, Atomia.Store.AspNetMvc.Adapters.CartProvider>();
            container.RegisterType<IContactDataProvider, Atomia.Store.AspNetMvc.Adapters.ContactDataProvider>();
            container.RegisterType<ICurrencyPreferenceProvider, Atomia.Store.AspNetMvc.Adapters.CurrencyPreferenceProvider>();
            container.RegisterType<ICurrencyFormatter, Atomia.Store.AspNetMvc.Adapters.CurrencyFormatter>();
            container.RegisterType<IResourceProvider, Atomia.Store.WebBase.Adapters.ResourceProvider>();
            container.RegisterType<ICustomerTypeProvider, Atomia.Store.AspNetMvc.Adapters.CustomerTypeProvider>();
            container.RegisterType<PaymentUrlProvider, Atomia.Store.AspNetMvc.Adapters.PaymentUrlProvider>();
            container.RegisterType<IThemeNamesProvider, Atomia.Store.AspNetMvc.Adapters.ThemeNamesProvider>();
            container.RegisterType<ICountryProvider, Atomia.Store.PublicBillingApi.Adapters.CountryProvider>();
            container.RegisterType<IResellerProvider, Atomia.Store.PublicBillingApi.Adapters.ResellerProvider>();
            container.RegisterType<ILanguageProvider, Atomia.Store.PublicBillingApi.Adapters.LanguageProvider>();
            container.RegisterType<ICurrencyProvider, Atomia.Store.PublicBillingApi.Adapters.CurrencyProvider>();
            container.RegisterType<IProductProvider, Atomia.Store.PublicBillingApi.Adapters.ProductProvider>();
            container.RegisterType<IPaymentMethodsProvider, Atomia.Store.PublicBillingApi.Adapters.PaymentMethodsProvider>();
            container.RegisterType<IItemPresenter, Atomia.Store.AspNetMvc.Adapters.ItemPresenter>();
            container.RegisterType<ILanguagePreferenceProvider, Atomia.Store.AspNetMvc.Adapters.LanguagePreferenceProvider>();
            container.RegisterType<IResellerIdentifierProvider, Atomia.Store.AspNetMvc.Adapters.ResellerIdentifierProvider>();
            container.RegisterType<ITermsOfServiceProvider, Atomia.Store.PublicBillingApi.Adapters.TermsOfServiceProvider>();
            container.RegisterType<ICartPricingService, Atomia.Store.PublicBillingApi.Adapters.CartPricingProvider>("apiPricingService");
            container.RegisterType<ICartPricingService, Atomia.Store.PublicBillingApi.Adapters.SetupFeeCartPricingService>(
                new InjectionConstructor(
                    new ResolvedParameter<ICartPricingService>("apiPricingService"),
                    new ResolvedParameter<IResellerProvider>(),
                    new ResolvedParameter<Atomia.Web.Plugin.ProductsProvider.IProductsProvider>()));

            // Public billing api helpers
            container.RegisterType<PublicBillingApiClient, PublicBillingApiClient>();
            container.RegisterType<PublicBillingApiProxy, PublicBillingApiProxy>();
            container.RegisterType<RenewalPeriodProvider, RenewalPeriodProvider>();
            container.RegisterType<IResellerDataProvider, ResellerDataProvider>("apiProvider");
            container.RegisterType<IResellerDataProvider, CachedResellerDataProvider>(
                new InjectionConstructor(
                    new ResolvedParameter<IResellerDataProvider>("apiProvider"),
                    new ResolvedParameter<IResellerIdentifierProvider>()));
        }

        /// <summary>
        /// Register product listing providers.
        /// </summary>
        protected void RegisterProductListingProviders()
        {
            container.RegisterType<IProductListProvider, Atomia.Store.PublicBillingApi.Adapters.CategoryProductsProvider>("Category");
        }

        /// <summary>
        /// Register basic domains provider and any decorators.
        /// </summary>
        protected void RegisterDomainsProvider()
        {
            container.RegisterType<IDomainsProvider, Atomia.Store.PublicBillingApi.Adapters.DomainsProvider>("apiProvider");
            container.RegisterType<IDomainsProvider, Atomia.Store.Themes.Default.Adapters.PremiumDomainsProvider>(
                new InjectionConstructor(new ResolvedParameter<IDomainsProvider>("apiProvider")));
        }

        /// <summary>
        /// Register overridable Asp.NET MVC view models.
        /// </summary>
        protected void RegisterViewModels()
        {
            container.RegisterType<DomainsViewModel, DomainsViewModel>();
            container.RegisterType<ProductListingViewModel, ProductListingViewModel>();
            container.RegisterType<ProductListingModel, ProductListingModel>();
            container.RegisterType<AccountViewModel, DefaultAccountViewModel>();
            container.RegisterType<CheckoutViewModel, DefaultCheckoutViewModel>();
        }

        /// <summary>
        /// Register payment plugins, forms and handlers
        /// </summary>
        protected void RegisterPaymentMethods()
        {
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.AdyenHpp.AdyenHppGuiPlugin>("AdyenHpp");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.DibsFlexwin.DibsFlexwinGuiPlugin>("DibsFlexwin");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.Invoice.PayWithInvoiceGuiPlugin>("PayWithInvoice");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.PayExRedirect.PayExRedirectGuiPlugin>("PayExRedirect");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.PayPal.PayPalGuiPlugin>("PayPal");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.WorldPay.WorldPayGuiPlugin>("WorldPay");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.WorldPayXml.WorldPayXmlGuiPlugin>("WorldPayXml");

            container.RegisterType<PaymentMethodForm, Atomia.Store.Payment.Invoice.PayWithInvoiceForm>("PayWithInvoice");

            container.RegisterType<PaymentDataHandler, Atomia.Store.Payment.AdyenHpp.AdyenHppHandler>("AdyenHpp");
            container.RegisterType<PaymentDataHandler, Atomia.Store.Payment.DibsFlexwin.DibsFlexwinHandler>("DibsFlexwin");
            container.RegisterType<PaymentDataHandler, Atomia.Store.Payment.Invoice.PayWithInvoiceHandler>("PayWithInvoice");
            container.RegisterType<PaymentDataHandler, Atomia.Store.Payment.PayExRedirect.PayExRedirectHandler>("PayExRedirect");
            container.RegisterType<PaymentDataHandler, Atomia.Store.Payment.PayPal.PayPalHandler>("PayPal");
            container.RegisterType<PaymentDataHandler, Atomia.Store.Payment.WorldPay.WorldPayHandler>("WorldPay");
            container.RegisterType<PaymentDataHandler, Atomia.Store.Payment.WorldPayXml.WorldPayXmlHandler>("WorldPayXml");
        }

        /// <summary>
        /// Register order placement serivce and related handlers.
        /// </summary>
        protected void RegisterOrderHandling()
        {
            // Transaction data handler
            container.RegisterType<TransactionDataHandler, Atomia.Store.PublicOrderHandlers.TransactionDataHandlers.RequestParamsHandler>("RequestParamsHandler");

            // Order data handlers and order placement service
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.ResellerHandler>("Reseller");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.LanguageHandler>("LanguageHandler");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.CurrencyHandler>("Currency");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.ContactDataHandlers.MainContactDataHandler>("MainContact");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.ContactDataHandlers.BillingContactDataHandler>("BillingContact");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.CampaignCodeHandler>("CampaignCode");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.IpAddressHandler>("IpAddress");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.CartItemHandlers.RegisterDomainHandler>("RegisterDomain");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.CartItemHandlers.TransferDomainHandler>("TransferDomain");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.CartItemHandlers.OwnDomainHandler>("OwnDomain");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.CartItemHandlers.DefaultHandler>("Default");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.CartItemHandlers.SetupFeesHandler>("SetupFees");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.CartItemHandlers.RemovePostOrderHandler>("RemovePostOrder");

            // We resolve the OrderPlacementService parameters manually to control the order the OrderHandlers are applied.
            container.RegisterType<IOrderPlacementService, Atomia.Store.PublicBillingApi.Adapters.OrderPlacementService>(
                new InjectionConstructor(
                    new ResolvedParameter<PaymentUrlProvider>(),
                    new ResolvedParameter<IProductProvider>(),
                    new ResolvedParameter<RenewalPeriodProvider>(),
                    new ResolvedArrayParameter<OrderDataHandler>(
                        new ResolvedParameter<OrderDataHandler>("Reseller"),
                        new ResolvedParameter<OrderDataHandler>("LanguageHandler"),
                        new ResolvedParameter<OrderDataHandler>("Currency"),
                        new ResolvedParameter<OrderDataHandler>("MainContact"),
                        new ResolvedParameter<OrderDataHandler>("BillingContact"),
                        new ResolvedParameter<OrderDataHandler>("CampaignCode"),
                        new ResolvedParameter<OrderDataHandler>("IpAddress"),
                        new ResolvedParameter<OrderDataHandler>("RegisterDomain"),
                        new ResolvedParameter<OrderDataHandler>("TransferDomain"),
                        new ResolvedParameter<OrderDataHandler>("OwnDomain"),
                        new ResolvedParameter<OrderDataHandler>("SetupFees"),

                        // This is a good position for TLD specific handlers.

                        // Default should be placed after all other handlers that add items form the cart to the order, or there is risk of adding the same item twice.
                        new ResolvedParameter<OrderDataHandler>("Default"),

                        // This is a good position for handlers that add extra items depending on other items in cart, e.g. like HST-APPY in old order page.

                        // RemovePostOrder should be placed last to make sure any added postal fees are removed, since they will be added by Atomia Billing.
                        new ResolvedParameter<OrderDataHandler>("RemovePostOrder")
                    ),
                    new ResolvedParameter<IEnumerable<PaymentDataHandler>>(),
                    new ResolvedParameter<IEnumerable<TransactionDataHandler>>(),
                    new ResolvedParameter<ILogger>(),
                    new ResolvedParameter<PublicBillingApiProxy>()
                )
            );

            // These are required since Unity does not handle IEnumerable<T> automatically.
            container.RegisterType<IEnumerable<PaymentDataHandler>, PaymentDataHandler[]>();
            container.RegisterType<IEnumerable<OrderDataHandler>, OrderDataHandler[]>();
            container.RegisterType<IEnumerable<TransactionDataHandler>, TransactionDataHandler[]>();
        }

        /// <summary>
        /// Fake adapters that use static data instead of relying on public order api being available.
        /// </summary>
        protected void RegisterFakeAdapters()
        {
            container.RegisterType<ILanguageProvider, Atomia.Store.Fakes.Adapters.FakeLanguageProvider>();
            container.RegisterType<IResellerProvider, Atomia.Store.Fakes.Adapters.FakeResellerProvider>();
            container.RegisterType<ICountryProvider, Atomia.Store.Fakes.Adapters.FakeCountryProvider>();
            container.RegisterType<IProductListProvider, Atomia.Store.Fakes.Adapters.FakeCategoryProductsProvider>("Category");
            container.RegisterType<IProductProvider, Atomia.Store.Fakes.Adapters.FakeCategoryProductsProvider>();
            container.RegisterType<ICartPricingService, Atomia.Store.Fakes.Adapters.FakePricingProvider>();
            container.RegisterType<IPaymentMethodsProvider, Atomia.Store.Fakes.Adapters.FakePaymentMethodsProvider>();
            container.RegisterType<IResellerIdentifierProvider, Atomia.Store.Fakes.Adapters.FakeRootResellerIdentifierProvider>();
            container.RegisterType<IDomainsProvider, Atomia.Store.Fakes.Adapters.FakePremiumDomainsProvider>();
            container.RegisterType<IOrderPlacementService, Atomia.Store.Fakes.Adapters.FakeOrderPlacementService>();
            container.RegisterType<ICurrencyProvider, Atomia.Store.Fakes.Adapters.FakeCurrencyProvider>();
        }
    }
}
