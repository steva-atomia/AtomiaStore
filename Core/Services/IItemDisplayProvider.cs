using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atomia.Store.Core
{
    public interface IItemDisplayProvider
    {
        string GetName(Item item);

        string GetDescription(Item item);
    }
}
