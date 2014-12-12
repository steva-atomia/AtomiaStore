using Atomia.Store.Core;
using Atomia.Store.Themes.Default.ViewModels;
using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.ViewModels;
using Atomia.Store.AspNetMvc.Services;
using Atomia.Store.Services.ActionTrail;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Unity.Mvc5;

// TODO: Remove this when all fake services have been switched out.
using Atomia.Store.Services.Fakes;

namespace Atomia.Store.Themes.Default
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IModelProvider, ModelProvider>();
            container.RegisterType<IModelBinderProvider, ModelBinderProvider>();

            container.RegisterType<DomainsViewModel, DefaultDomainsViewModel>();
            container.RegisterType<ProductsViewModel, DefaultProductsViewModel>();

            container.RegisterType<IDomainSearchService, FakeDomainSearchService>();
            container.RegisterType<ILogger, Logger>();

            container.RegisterType<ICartProvider, CartProvider>();
            container.RegisterType<ICartPricingService, FakePricingProvider>();
            container.RegisterType<ICurrencyProvider, CurrencyProvider>();

            container.RegisterType<IItemDisplayProvider, FakeItemDisplayProvider>();
            container.RegisterType<IProductsProvider, FakeProductsProvider>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
