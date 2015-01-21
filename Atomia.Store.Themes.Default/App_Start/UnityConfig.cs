using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.AspNetMvc.Providers;
using Atomia.Store.Core;
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

            container.RegisterType<IViewEngine, Atomia.Store.AspNetMvc.Infrastructure.RazorThemeViewEngine>("RazorThemeViewEngine");
            container.RegisterType<IThemeNamesProvider, Atomia.Store.Fakes.Adapters.FakeThemeNamesProvider>();

            container.RegisterType<IModelBinderProvider, Atomia.Store.AspNetMvc.Infrastructure.ModelBinderProvider>();
            
            container.RegisterType<ILogger, Atomia.Store.ActionTrail.Adapters.Logger>();
            container.RegisterType<ICartProvider, Atomia.Store.AspNetMvc.Adapters.CartProvider>();
            container.RegisterType<ICurrencyProvider, Atomia.Store.AspNetMvc.Adapters.CurrencyProvider>();
            container.RegisterType<ICurrencyFormatter, Atomia.Store.AspNetMvc.Adapters.CurrencyFormatter>();
            container.RegisterType<IResourceProvider, Atomia.Store.WebBase.Adapters.ResourceProvider>();
            container.RegisterType<ICartPricingService, Atomia.Store.Fakes.Adapters.FakePricingProvider>();
            container.RegisterType<IItemPresenter, Atomia.Store.Fakes.Adapters.FakeItemPresenter>();

            // ViewModels
            container.RegisterType<CategoryDataViewModel, Atomia.Store.Themes.Default.Models.CategoryViewModel>();
            container.RegisterType<DomainsViewModel, Atomia.Store.Themes.Default.Models.DomainsViewModel>();

            // Product providers
            container.RegisterType<AllProductsProvider, Atomia.Store.Fakes.Adapters.FakeProductsProvider>();
            container.RegisterType<DomainSearchProvider, Atomia.Store.Fakes.Adapters.FakeDomainSearchProvider>();
            container.RegisterType<CategoryProvider, Atomia.Store.AspNetMvc.Providers.SimpleCategoryProvider>();
            container.RegisterType<DomainsProvider, Atomia.Store.AspNetMvc.Providers.PremiumDomainsProvider>();

            container.LoadConfiguration();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
