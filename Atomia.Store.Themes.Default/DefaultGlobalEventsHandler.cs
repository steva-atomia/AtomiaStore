using Atomia.Store.AspNetMvc.Controllers;
using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.Core;
using Atomia.Web.Base.Configs;
using Atomia.Web.Plugin.Validation.ValidationAttributes;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity.Mvc5;


namespace Atomia.Store.Themes.Default
{
    /// <summary>
    /// A <see cref="Atomia.Store.AspNetMvc.Infrastructure.GlobalEventsHandler"/> for bootstrapping Default theme.
    /// </summary>
    public class DefaultGlobalEventsHandler : GlobalEventsHandler
    {
        /// <summary>
        /// Place for subclasses to override Unity configuration.
        /// </summary>
        /// <param name="container">The Unity container that will be set as Asp.Net MVC dependency resolver</param>
        protected virtual void RegisterConfiguration(UnityContainer container)
        {
            
        }

        /// <summary>
        /// Register configuration for Unity, filters, routes, bundles, order flow and model validation. Add legacy global settings configuration.
        /// </summary>
        public override void  Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();

            var container = new UnityContainer();
            var orderFlows = new OrderFlowCollection();

            UnityConfig.RegisterComponents(container);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            OrderFlowConfig.RegisterOrderFlows(GlobalOrderFlows.OrderFlows);

            RegisterConfiguration(container);

            container.LoadConfiguration();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            // RazorThemeViewEngine works with one instance per theme and lets the view engine selection 
            // algorithm do its work to find a view.
            var themeNamesProvider = container.Resolve<IThemeNamesProvider>();
            foreach (var theme in themeNamesProvider.GetActiveThemeNames())
            {
                ViewEngines.Engines.Add(new RazorThemeViewEngine(theme));
            }

            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(CustomerValidationAttribute), typeof(CustomerValidationAttribute.CustomerValidator));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(AtomiaRegularExpressionAttribute), typeof(AtomiaRegularExpressionValidator));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(AtomiaRequiredAttribute), typeof(AtomiaRequiredValidator));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(AtomiaStringLengthAttribute), typeof(AtomiaStringLengthValidator));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(AtomiaRangeAttribute), typeof(AtomiaRangeValidator));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(AtomiaUsernameAttribute), typeof(AtomiaUsernameAttribute.AtomiaUsernameValidator));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(AtomiaUsernameRequiredAttribute), typeof(AtomiaUsernameRequiredValidator));

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
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                // We are making an exception to using DI/service location of adapters here, since they have not yet
                // been registered with the Unity container. Meaning this needs to be changed manually if you change the
                // IThemeNamesProvider implementation.
                var themeNamesProvider = new Atomia.Store.AspNetMvc.Adapters.ThemeNamesProvider();

                // Register "theme" key in session, which is required by legacy resource string handling in Atomia.Web.Base
                HttpContext.Current.Session["theme"] = themeNamesProvider.GetCurrentThemeName();
            }
        }

        /// <summary>
        /// Set up error handling to log exceptions and defer to error controller or as last resort static HTML error page.
        /// </summary>
        public override void Application_Error(object sender, EventArgs e)
        {
            if (!HttpContext.Current.IsCustomErrorEnabled)
            {
                return;
            }

            try
            {
                var ex = HttpContext.Current.Server.GetLastError();
                var httpException = ex as HttpException;

                var logger = DependencyResolver.Current.GetService<ILogger>();
                logger.LogException(ex, string.Format("Caught unhandled exception in Store.\r\n {0}", ex.Message + "\r\n" + ex.StackTrace));

                var routeData = new System.Web.Routing.RouteData();
                routeData.Values.Add("controller", "Error");

                if (httpException != null)
                {
                    switch (httpException.GetHttpCode())
                    {
                        case 401:
                            routeData.Values.Add("action", "Forbidden");
                            break;
                        case 404:
                            routeData.Values.Add("action", "NotFound");
                            break;
                        default:
                            routeData.Values.Add("action", "InternalServerError");
                            break;
                    }
                }
                else
                {
                    routeData.Values.Add("action", "InternalServerError");
                }

                routeData.Values.Add("error", ex);

                HttpContext.Current.Server.ClearError();

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.TrySkipIisCustomErrors = true;
                
                IController errorController = new ErrorController();
                errorController.Execute(new RequestContext(new HttpContextWrapper(HttpContext.Current), routeData));
            }
            catch
            {
                // This is a last ditch effort in case something goes wrong with the regular error handling.
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                HttpContext.Current.Response.ContentType = "text/html";
                HttpContext.Current.Response.WriteFile("~/Content/Error.html");
            }
        }
    }
}
