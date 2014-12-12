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
                defaults: new { controller = "Products", action = "ListCategory", category = "Hosting" }
            );

            routes.MapRoute(
                name: "Addons",
                url: "Addons",
                defaults: new { controller = "Products", action = "ListCategory", category = "Extra service" }
            );

            routes.MapRoute(
                name: "Domains",
                url: "Domains",
                defaults: new { controller = "Domains", action = "Index" }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Domains", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
