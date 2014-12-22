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
                    controller = "Products",
                    action = "ListProducts",
                    productsProviderName = "category",
                    searchQuery = new ProductSearchQuery
                    {
                        Terms = new List<SearchTerm>
                        {
                            new SearchTerm
                            {
                                Key = "category",
                                Value = "Hosting"
                            }
                        },
                    }
                }
            );

            routes.MapRoute(
                name: "Addons",
                url: "Addons",
                defaults: new
                {
                    controller = "Products",
                    action = "ListProducts",
                    productsProviderName = "category",
                    searchQuery = new ProductSearchQuery
                    {
                        Terms = new List<SearchTerm>
                        {
                            new SearchTerm
                            {
                                Key = "category",
                                Value = "Extra service"
                            }
                        },
                    }
                }
            );

            routes.MapRoute(
                name: "Domains",
                url: "Domains",
                defaults: new { controller = "Products", action = "SearchForProducts", productsProviderName = "domains", viewName = "Domains" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}"
            );
        }
    }
}
