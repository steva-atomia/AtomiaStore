using Atomia.Store.Core;
using System.Web.Mvc;
using System.Web.Routing;

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
                defaults: new { controller = "Products", action = "ListCategory", category = "Hosting", productProvider = "category" }
            );

            routes.MapRoute(
                name: "Addons",
                url: "Addons",
                defaults: new { controller = "Products", action = "ListProducts", productProvider = "addons" }
            );

            routes.MapRoute(
                name: "Domains",
                url: "Domains",
                defaults: new { controller = "Products", action = "ListProducts", productProvider = "domains", viewName="Domains" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}"
            );
        }
    }
}
