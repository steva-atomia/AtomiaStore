using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Atomia.OrderPage.UI.Infrastructure;
using Atomia.Web.Base.Configs;

namespace Atomia.OrderPage.Themes.Default
{
    public class DefaultGlobalEventsHandler : GlobalEventsHandler
    {
        public override void  Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();

            UnityConfig.RegisterComponents();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            foreach (GlobalSetting globalSetting in AppConfig.Instance.GlobalSettingsList)
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Application[globalSetting.Name] = globalSetting.Value;
                }
            }
        }

        public override void Session_Start(object sender, EventArgs e)
        {
            // FIXME: This is a temporary measure to add a theme to the session.
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["theme"] = "Default";
                }
            }
        }

        public override void Application_BeginRequest(object sender, EventArgs e)
        {
            Atomia.OrderPage.ActionTrail.OrderPageLogger.LogOrderPageException(new Exception("Hello? Yes, this is dog."));
            base.Application_BeginRequest(sender, e);
        }
    }
}
