using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using Atomia.OrderPage.Core.Services;
using Atomia.OrderPage.Services.WebPluginDomainSearch;
using Atomia.OrderPage.UI.Infrastructure;
using Atomia.OrderPage.UI.ViewModels;
using Atomia.OrderPage.Themes.Default.ViewModels;

namespace Atomia.OrderPage.Themes.Default
{
  public static class Bootstrapper
  {
    public static IUnityContainer Initialise()
    {
      var container = BuildUnityContainer();

      DependencyResolver.SetResolver(new UnityDependencyResolver(container));

      return container;
    }

    private static IUnityContainer BuildUnityContainer()
    {
      var container = new UnityContainer();

      // register all your components with the container here
      // it is NOT necessary to register your controllers

      // e.g. container.RegisterType<ITestService, TestService>();    
      RegisterTypes(container);

      return container;
    }

    public static void RegisterTypes(IUnityContainer container)
    {
        container.RegisterType<IModelProvider, ModelProvider>();
        container.RegisterType<IModelBinderProvider, ModelBinderProvider>();

        container.RegisterType<DomainsViewModel, DefaultDomainsViewModel>();

        container.RegisterType<IDomainSearchService, DomainSearchService>();
    }
  }
}