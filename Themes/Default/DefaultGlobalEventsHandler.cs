using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Atomia.OrderPage.Sdk.Infrastructure;

namespace Atomia.OrderPage.Themes.Default
{
    public class DefaultGlobalEventsHandler : GlobalEventsHandler
    {
        public override void  Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Bootstrapper.Initialise();
        }
    }
}
