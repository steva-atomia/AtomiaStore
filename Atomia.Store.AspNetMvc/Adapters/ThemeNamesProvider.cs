using Atomia.Store.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Atomia.Store.AspNetMvc.Adapters
{
    public class ThemeNamesProvider : IThemeNamesProvider
    {
        public IEnumerable<string> GetActiveThemeNames()
        {
            var themes = new List<string>();
            var themeSettings = ConfigurationManager.AppSettings["Themes"];

            if (!String.IsNullOrEmpty(themeSettings))
            {
                themes = themeSettings
                    .Split(',')
                    .Select(t => t.Trim())
                    .ToList();
            }

            return themes;
        }

        public string GetMainThemeName()
        {
            var themeNames = GetActiveThemeNames();

            var mainThemeName = themeNames.FirstOrDefault();

            if (String.IsNullOrEmpty(mainThemeName))
            {
                mainThemeName = "Default";
            }

            return mainThemeName;
        }
    }
}
