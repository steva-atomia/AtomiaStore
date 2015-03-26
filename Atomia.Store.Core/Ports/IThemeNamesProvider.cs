using System.Collections.Generic;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Interface for providing theme names.
    /// </summary>
    public interface IThemeNamesProvider
    {
        /// <summary>
        /// Get names of all themes that are considered active. Themes will be used in the order they are returned for finding views.
        /// </summary>
        /// <remarks>By convention the last theme in the list should be "Default".</remarks>
        /// <returns>The active theme names</returns>
        IEnumerable<string> GetActiveThemeNames();

        /// <summary>
        /// Get the name for the theme that is considered the "main" theme.
        /// </summary>
        /// <remarks>Used mainly by legacy components that only support one theme + the "Default" theme.</remarks>
        /// <returns>The name of the "main" theme.</returns>
        string GetMainThemeName();
    }
}
