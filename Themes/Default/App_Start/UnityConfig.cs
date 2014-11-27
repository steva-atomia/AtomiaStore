using Atomia.OrderPage.Core.Services;
using Atomia.OrderPage.Services.WebPluginDomainSearch;
using Atomia.OrderPage.Themes.Default.ViewModels;
using Atomia.OrderPage.UI.Infrastructure;
using Atomia.OrderPage.UI.ViewModels;
using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Unity.Mvc5;


namespace Atomia.OrderPage.Themes.Default
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IModelProvider, ModelProvider>();
            container.RegisterType<IModelBinderProvider, ModelBinderProvider>();

            container.RegisterType<DomainsViewModel, DefaultDomainsViewModel>();

            container.RegisterType<IDomainSearchService, DomainSearchService>();
            container.RegisterType<ILogger, Atomia.OrderPage.Services.ActionTrail.Logger>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}