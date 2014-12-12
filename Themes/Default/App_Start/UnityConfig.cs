using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
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
            container.RegisterType<ProductsViewModel, Atomia.Store.Themes.Default.Models.DefaultProductsViewModel>();

            // Services
            container.RegisterType<ILogger, Atomia.Store.Services.ActionTrail.Logger>();
            container.RegisterType<ICartProvider, Atomia.Store.AspNetMvc.Services.CartProvider>();
            container.RegisterType<ICurrencyProvider, Atomia.Store.Core.CurrencyProvider>();

            // Fakes
            container.RegisterType<IDomainSearchService, Atomia.Store.Services.Fakes.FakeDomainSearchService>();
            container.RegisterType<ICartPricingService, Atomia.Store.Services.Fakes.FakePricingProvider>();
            container.RegisterType<IItemDisplayProvider, Atomia.Store.Services.Fakes.FakeItemDisplayProvider>();
            container.RegisterType<IProductsProvider, Atomia.Store.Services.Fakes.FakeProductsProvider>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
