using Atomia.Store.Core;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Infrastructure
{
    /// <summary>
    /// An implementation of the standard RazorViewEngine that uses views from the Themes folder based on current theme name.
    /// </summary>
    /// <remarks>Stores with multiple themes should register an instance for each theme.</remarks>
    public sealed class RazorThemeViewEngine : RazorViewEngine
    {
        private readonly string themeName;
        private readonly IThemeNamesProvider themeNamesProvider = DependencyResolver.Current.GetService<IThemeNamesProvider>();

        public RazorThemeViewEngine(string themeName)
        {
            if (string.IsNullOrEmpty(themeName))
            {
                throw new ArgumentException("themeName is required.", "themeName");
            }

            this.themeName = themeName;

            var themeAreaLocations = new[]
            {
                "~/Themes/" + themeName + "/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Themes/" + themeName + "/Areas/{2}/Views/{1}/{0}.vbhtml",
                "~/Themes/" + themeName + "/Areas/{2}/Views/Shared/{0}.cshtml",
                "~/Themes/" + themeName + "/Areas/{2}/Views/Shared/{0}.vbhtml",
            };
            
            var themeViewLocations = new[]
            {
                "~/Themes/" + themeName + "/Views/{1}/{0}.cshtml",
                "~/Themes/" + themeName + "/Views/{1}/{0}.vbhtml",
                "~/Themes/" + themeName + "/Views/Shared/{0}.cshtml",
                "~/Themes/" + themeName + "/Views/Shared/{0}.vbhtml",
            };

            AreaViewLocationFormats = themeAreaLocations.ToArray();
            AreaMasterLocationFormats = themeAreaLocations.ToArray();
            AreaPartialViewLocationFormats = themeAreaLocations.ToArray();
            ViewLocationFormats = themeViewLocations.ToArray();
            MasterLocationFormats = themeViewLocations.ToArray();
            PartialViewLocationFormats = themeViewLocations.ToArray();

            FileExtensions = new[] { "cshtml", "vbhtml" };
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (this.themeName != themeNamesProvider.GetCurrentThemeName() && this.themeName != "Default")
            {
                // Just skip this theme, we didn't search any view locations.
                return new ViewEngineResult(new string[0]);
            }

            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            if (this.themeName != themeNamesProvider.GetCurrentThemeName() && this.themeName != "Default")
            {
                // Just skip this theme, we didn't search any view locations.
                return new ViewEngineResult(new string[0]);
            }

            return base.FindPartialView(controllerContext, partialViewName, useCache);
        }
    }
}
