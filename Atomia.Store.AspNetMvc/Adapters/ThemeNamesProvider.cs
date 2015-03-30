using Atomia.Store.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Atomia.Store.AspNetMvc.Adapters
{
    /// <summary>
    /// App settings backed <see cref="Atomia.Store.Core.IThemeNamesProvider"/>
    /// </summary>
    public sealed class ThemeNamesProvider : IThemeNamesProvider
    {
        /// <summary>
        /// Get active theme names from app setting "Themes", specified as comma separated list.
        /// </summary>
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

        /// <summary>
        /// Get first name from list of names in app setting "Themes", or "Default"
        /// </summary>
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
