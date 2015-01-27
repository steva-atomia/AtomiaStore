using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
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
            container.RegisterType<ILanguageProvider, Atomia.Store.Fakes.Adapters.FakeLanguageProvider>();
            container.RegisterType<IResellerProvider, Atomia.Store.Fakes.Adapters.FakeResellerProvider>();

            // ViewModels
            container.RegisterType<DomainViewModel, DomainViewModel>();
            container.RegisterType<ProductListingViewModel, ProductListingViewModel>();
            container.RegisterType<ProductListingDataModel, ProductListingDataModel>();
            container.RegisterType<AccountViewModel, DefaultAccountViewModel>();
            

            // Product providers
            container.RegisterType<IProductsProvider, Atomia.Store.Fakes.Adapters.FakeCategoryProductsProvider>();
            container.RegisterType<IDomainsProvider, Atomia.Store.Fakes.Adapters.FakePremiumDomainSearchProvider>();

            container.LoadConfiguration();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
