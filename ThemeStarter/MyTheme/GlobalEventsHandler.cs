using Microsoft.Practices.Unity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Atomia.Store.AspNetMvc.Infrastructure;


namespace $MyTheme$
{
    public class GlobalEventsHandler : Atomia.Store.Themes.Default.DefaultGlobalEventsHandler
    {
        protected override void RegisterConfiguration(UnityContainer container)
        {
            UnityConfig.RegisterComponents(container);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            OrderFlowConfig.RegisterOrderFlows(GlobalOrderFlows.OrderFlows);
        }
    }
}
