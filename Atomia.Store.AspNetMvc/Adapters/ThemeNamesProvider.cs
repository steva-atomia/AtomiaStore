using Atomia.Store.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

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
            var themeSettings = ConfigurationManager.AppSettings["ActiveThemes"];

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
        /// Get user's preferred theme, theme from "StartTheme" setting, or "Default" theme
        /// </summary>
        public string GetCurrentThemeName()
        {
            string currentTheme = null;

            var selectedTheme = (string)HttpContext.Current.Session["SelectedTheme"];
            var activeThemes = GetActiveThemeNames();
            if (!String.IsNullOrEmpty(selectedTheme) && activeThemes.Contains(selectedTheme))
            {
                currentTheme = selectedTheme;
            }

            if (String.IsNullOrEmpty(currentTheme))
            {
                var startTheme = ConfigurationManager.AppSettings["StartTheme"];

                if (!String.IsNullOrEmpty(startTheme) && activeThemes.Contains(startTheme))
                {
                    currentTheme = startTheme;
                }
            }

            if (String.IsNullOrEmpty(currentTheme))
            {
                currentTheme = "Default";
            }

            SetCurrentThemeName(currentTheme);

            return currentTheme;
        }

        /// <summary>
        /// Save user's current theme in session.
        /// </summary>
        public void SetCurrentThemeName(string themeName)
        {
            HttpContext.Current.Session["SelectedTheme"] = String.IsNullOrEmpty(themeName) ? "" : themeName;
        }
    }
}
