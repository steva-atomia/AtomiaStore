using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atomia.Store.Core
{
    public interface IThemeNamesProvider
    {
        IEnumerable<string> GetActiveThemeNames();
    }
}
