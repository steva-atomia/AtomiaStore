using Atomia.Store.Core;
using Atomia.Store.Services.WebPluginDomainSearch;
using Atomia.Store.Themes.Default.ViewModels;
using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.ViewModels;
using Atomia.Store.AspNetMvc.Services;
using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Unity.Mvc5;


namespace Atomia.Store.Themes.Default
{
    public static class UnityConfig
    {
        public class FakePricingProvider : ICartPricingService
        {
            public Cart CalculatePricing(Cart cart)
            {
                return cart;
            }
        }


        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IModelProvider, ModelProvider>();
            container.RegisterType<IModelBinderProvider, ModelBinderProvider>();

            container.RegisterType<DomainsViewModel, DefaultDomainsViewModel>();

            container.RegisterType<IDomainSearchService, DomainSearchService>();
            container.RegisterType<ILogger, Atomia.Store.Services.ActionTrail.Logger>();

            container.RegisterType<ICartProvider, CartProvider>();
            container.RegisterType<ICartPricingService, FakePricingProvider>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}