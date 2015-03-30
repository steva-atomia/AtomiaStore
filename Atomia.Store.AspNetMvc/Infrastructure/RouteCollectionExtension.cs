using System.Web.Mvc;
using System.Web.Routing;

namespace Atomia.Store.AspNetMvc.Infrastructure
{
    /// <summary>
    /// Extensions to <see cref="System.Web.Routing.RouteCollection"/>
    /// </summary>
    public static class RouteCollectionExtension
    {
        /// <summary>
        /// Map a route that should be part of an <see cref="OrderFlow"/>. Makes the route name avaiable for filters etc.
        /// </summary>
        public static Route MapOrderFlowRoute(this RouteCollection routes, string name, string url, object defaults)
        {
            Route route = routes.MapRoute(name, url, defaults);
            route.DataTokens = new RouteValueDictionary();

            route.DataTokens["Name"] = name;
            
            return route;
        }
    }
}
