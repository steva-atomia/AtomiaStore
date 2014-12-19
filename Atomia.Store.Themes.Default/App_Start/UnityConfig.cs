using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.AspNetMvc;
using Atomia.Store.Core;
using Microsoft.Practices.Unity;
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

            // Services
            container.RegisterType<ILogger, Atomia.Store.ActionTrail.Logger>();
            container.RegisterType<ICartProvider, Atomia.Store.AspNetMvc.Adapters.CartProvider>();
            container.RegisterType<ICurrencyProvider, Atomia.Store.AspNetMvc.Adapters.CurrencyProvider>();
            container.RegisterType<ICurrencyFormatter, Atomia.Store.AspNetMvc.Adapters.CurrencyFormatter>();
            container.RegisterType<IResourceProvider, Atomia.Store.WebBase.ResourceProvider>();

            // Fakes
            container.RegisterType<ICartPricingService, Atomia.Store.Fakes.FakePricingProvider>();
            container.RegisterType<IProductsProvider, Atomia.Store.Fakes.FakeProductsProvider>();
            container.RegisterType<IProductsProvider, Atomia.Store.Core.AddonsProvider>("addons");

            container.RegisterType<IItemPresenter, Atomia.Store.Fakes.FakeItemPresenter>();
            container.RegisterType<IThemeNamesProvider, Atomia.Store.Fakes.FakeThemeNamesProvider>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
