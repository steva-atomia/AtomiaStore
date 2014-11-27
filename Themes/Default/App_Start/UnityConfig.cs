using Atomia.Store.Core.Services;
using Atomia.Store.Services.WebPluginDomainSearch;
using Atomia.Store.Themes.Default.ViewModels;
using Atomia.Store.UI.Infrastructure;
using Atomia.Store.UI.ViewModels;
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

            container.RegisterType<IModelProvider, ModelProvider>();
            container.RegisterType<IModelBinderProvider, ModelBinderProvider>();

            container.RegisterType<DomainsViewModel, DefaultDomainsViewModel>();

            container.RegisterType<IDomainSearchService, DomainSearchService>();
            container.RegisterType<ILogger, Atomia.Store.Services.ActionTrail.Logger>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}