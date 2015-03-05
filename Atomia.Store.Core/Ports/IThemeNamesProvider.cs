using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public interface IThemeNamesProvider
    {
        IEnumerable<string> GetActiveThemeNames();

        string GetMainThemeName();
    }
}
