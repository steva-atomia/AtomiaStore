using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.AspNetMvc.Services;

namespace Atomia.Store.Services.Fakes
{
    public class FakeItemPresenter: IItemPresenter
    {
        public string GetName(IPresentableItem item)
        {
            return "Bloop";
        }

        public string GetDescription(IPresentableItem item)
        {
            return "Bloop description";
        }
    }
}
