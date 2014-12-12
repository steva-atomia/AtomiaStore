using Atomia.Store.Core;
using Atomia.Store.Services.WebPluginDomainSearch;
using Atomia.Store.Themes.Default.ViewModels;
using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.ViewModels;
using Atomia.Store.AspNetMvc.Services;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web.Mvc;
using Unity.Mvc5;


namespace Atomia.Store.Themes.Default
{
    public static class UnityConfig
    {
        internal class FakePricingProvider : ICartPricingService
        {
            public Cart CalculatePricing(Cart cart)
            {
                return cart;
            }
        }

        internal class FakeItemDisplayProvider : IItemDisplayProvider
        {
            public string GetName(Item item)
            {
                var domainNameAttr = item.CustomAttributes.First(ca => ca.Name == "DomainName");

                if (domainNameAttr != null)
                {
                    return domainNameAttr.Values.First();
                }

                return item.ArticleNumber;
            }

            public string GetDescription(Item item)
            {
                return "Description of article " + item.ArticleNumber;
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
            container.RegisterType<ICurrencyProvider, CurrencyProvider>();

            container.RegisterType<IItemDisplayProvider, FakeItemDisplayProvider>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
