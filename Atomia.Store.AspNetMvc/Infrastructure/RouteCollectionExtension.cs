using System.Web.Mvc;
using System.Web.Routing;

namespace Atomia.Store.AspNetMvc.Infrastructure
{
    public static class RouteCollectionExtension
    {
        public static Route MapOrderFlowRoute(this RouteCollection routes, string name, string url, object defaults)
        {
            Route route = routes.MapRoute(name, url, defaults);
            route.DataTokens = new RouteValueDictionary();

            route.DataTokens["Name"] = name;
            
            return route;
        }
    }
}
