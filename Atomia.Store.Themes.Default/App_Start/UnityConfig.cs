using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.AspNetMvc;
using Atomia.Store.Core;
using Microsoft.Practices.Unity;
using System;
using System.Web.Mvc;
using Unity.Mvc5;

namespace Atomia.Store.Themes.Default
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // ViewModels
            container.RegisterType<IModelProvider, Atomia.Store.AspNetMvc.Infrastructure.ModelProvider>();
            container.RegisterType<IModelBinderProvider, Atomia.Store.AspNetMvc.Infrastructure.ModelBinderProvider>();
            container.RegisterType<DomainsViewModel, Atomia.Store.Themes.Default.Models.DefaultDomainsViewModel>();
            container.RegisterType<ListProductsViewModel, Atomia.Store.Themes.Default.Models.DefaultListProductsViewModel>();

            // Themes
            container.RegisterType<IViewEngine, Atomia.Store.AspNetMvc.Infrastructure.RazorThemeViewEngine>("RazorThemeViewEngine");

            // Ports
            container.RegisterType<ILogger, Atomia.Store.ActionTrail.Adapters.Logger>();
            container.RegisterType<ICartProvider, Atomia.Store.AspNetMvc.Adapters.CartProvider>();
            container.RegisterType<ICurrencyProvider, Atomia.Store.AspNetMvc.Adapters.CurrencyProvider>();
            container.RegisterType<ICurrencyFormatter, Atomia.Store.AspNetMvc.Adapters.CurrencyFormatter>();
            container.RegisterType<IResourceProvider, Atomia.Store.WebBase.Adapters.ResourceProvider>();

            var productsProviderFactory = new InjectionFactory(unityContainer => new Func<string, IProductsProvider>(
                    name => unityContainer.Resolve<IProductsProvider>(name)));

            container.RegisterType<Func<string, IProductsProvider>>(productsProviderFactory);

            // Fakes
            container.RegisterType<ICartPricingService, Atomia.Store.Fakes.Adapters.FakePricingProvider>();
            container.RegisterType<IProductsProvider, Atomia.Store.Fakes.Adapters.FakeProductsProvider>();
            container.RegisterType<IProductsProvider, Atomia.Store.Fakes.Adapters.FakeProductsProvider>("category");
            container.RegisterType<IProductsProvider, Atomia.Store.Fakes.Adapters.AddonProductsProvider>("addons");
            container.RegisterType<IProductsProvider, Atomia.Store.Fakes.Adapters.DomainSearchProductsProvider>("domains");
            container.RegisterType<IItemPresenter, Atomia.Store.Fakes.Adapters.FakeItemPresenter>();
            container.RegisterType<IThemeNamesProvider, Atomia.Store.Fakes.Adapters.FakeThemeNamesProvider>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
