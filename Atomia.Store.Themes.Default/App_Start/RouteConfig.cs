using Atomia.Store.Core;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;

namespace Atomia.Store.Themes.Default
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Hosting",
                url: "Hosting",
                defaults: new
                {
                    controller = "Category",
                    action = "Index",
                    viewName = "HostingPackages"
                }
            );

            routes.MapRoute(
                name: "Addons",
                url: "Addons",
                defaults: new
                {
                    controller = "Category",
                    action = "Index"
                }
            );

            routes.MapRoute(
                name: "Domains",
                url: "Domains/{action}",
                defaults: new { controller = "Domains", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Domains", action = "Index" }
            );
        }
    }
}
