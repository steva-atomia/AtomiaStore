using Atomia.Store.Core;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace Atomia.Store.AspNetMvc.Infrastructure
{
    public class RazorThemeViewEngine : RazorViewEngine
    {
        public RazorThemeViewEngine(IThemeNamesProvider themeNamesProvider)
        {
            var themes = themeNamesProvider.GetActiveThemeNames();

            var areaViewLocations = new List<string>();
            var areaMasterLocations = new List<string>();
            var areaPartialViewLocations = new List<string>();
            var viewLocations = new List<string>();
            var masterLocations = new List<string>();
            var partialViewLocations = new List<string>();

            foreach (var theme in themes)
            {
                var themeAreaLocations = new[]
                {
                    "~/Themes/" + theme + "/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Themes/" + theme + "/Areas/{2}/Views/{1}/{0}.vbhtml",
                    "~/Themes/" + theme + "/Areas/{2}/Views/Shared/{0}.cshtml",
                    "~/Themes/" + theme + "/Areas/{2}/Views/Shared/{0}.vbhtml",
                };
                areaViewLocations.AddRange(themeAreaLocations);
                areaMasterLocations.AddRange(themeAreaLocations);
                areaPartialViewLocations.AddRange(themeAreaLocations);

                var themeViewLocations = new[]
                {
                    "~/Themes/" + theme + "/Views/{1}/{0}.cshtml",
                    "~/Themes/" + theme + "/Views/{1}/{0}.vbhtml",
                    "~/Themes/" + theme + "/Views/Shared/{0}.cshtml",
                    "~/Themes/" + theme + "/Views/Shared/{0}.vbhtml",
                };
                viewLocations.AddRange(themeViewLocations);
                masterLocations.AddRange(themeViewLocations);
                partialViewLocations.AddRange(themeViewLocations);
            }

            var standardAreaLocations = new List<string>
            {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };
            AreaViewLocationFormats = areaViewLocations.Concat(standardAreaLocations).ToArray();
            AreaMasterLocationFormats = areaMasterLocations.Concat(standardAreaLocations).ToArray();
            AreaPartialViewLocationFormats = areaPartialViewLocations.Concat(standardAreaLocations).ToArray();

            var standardLocations = new List<string>
            {
                // Standard
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml"
            };
            ViewLocationFormats = viewLocations.Concat(standardLocations).ToArray();
            MasterLocationFormats = masterLocations.Concat(standardLocations).ToArray();
            PartialViewLocationFormats = partialViewLocations.Concat(standardLocations).ToArray();

            FileExtensions = new[]
            {
                "cshtml",
                "vbhtml",
            };
        }
    }
}
