using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using Atomia.OrderPage.Core.Services;
using Atomia.OrderPage.Services.WebPluginDomainSearch;
using Atomia.OrderPage.UI.Infrastructure;
using Atomia.OrderPage.UI.ViewModels;
using Atomia.OrderPage.Themes.Default.ViewModels;


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
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}