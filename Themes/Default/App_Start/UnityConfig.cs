using Atomia.Store.Core;
using Atomia.Store.Services.WebPluginDomainSearch;
using Atomia.Store.Themes.Default.ViewModels;
using Atomia.Store.UI.Infrastructure;
using Atomia.Store.UI.ViewModels;
using Atomia.Store.UI.Storage;
using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Unity.Mvc5;


namespace Atomia.Store.Themes.Default
{
    public static class UnityConfig
    {
        public class FakeProductNameProvider : IProductNameProvider
        {
            public string GetProductName(string articleNumber)
            {
                return "Bloop";
            }
        }

        public class FakePricingProvider : ICartPricingProvider
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

            container.RegisterType<ICartRepository, CartRepository>();
            container.RegisterType<IProductNameProvider, FakeProductNameProvider>();
            container.RegisterType<ICartPricingProvider, FakePricingProvider>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}