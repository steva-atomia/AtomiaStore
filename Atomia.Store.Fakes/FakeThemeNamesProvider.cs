using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.AspNetMvc.Infrastructure;

namespace Atomia.Store.Fakes
{
    public class FakeThemeNamesProvider : IThemeNamesProvider
    {
        public IEnumerable<string> GetActiveThemeNames()
        {
            return new List<string> { "Bloop" };
        }
    }
}
