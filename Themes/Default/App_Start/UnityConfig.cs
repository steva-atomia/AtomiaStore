using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.AspNetMvc.Services;
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
            container.RegisterType<ListCategoryViewModel, Atomia.Store.Themes.Default.Models.DefaultListCategoryViewModel>();

            // Services
            container.RegisterType<ILogger, Atomia.Store.Services.ActionTrail.Logger>();
            container.RegisterType<ICartProvider, Atomia.Store.AspNetMvc.Services.CartProvider>();
            container.RegisterType<ICurrencyProvider, Atomia.Store.AspNetMvc.Services.CurrencyProvider>();
            container.RegisterType<ICurrencyFormatter, Atomia.Store.AspNetMvc.Services.CurrencyFormatter>();

            // Fakes
            container.RegisterType<IDomainSearchService, Atomia.Store.Services.Fakes.FakeDomainSearchService>();
            container.RegisterType<ICartPricingService, Atomia.Store.Services.Fakes.FakePricingProvider>();
            container.RegisterType<IProductsProvider, Atomia.Store.Services.Fakes.FakeProductsProvider>();
            container.RegisterType<IItemPresenter, Atomia.Store.Services.Fakes.FakeItemPresenter>();
            container.RegisterType<IResourceProvider, Atomia.Store.Services.Fakes.FakeResourceProvider>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
