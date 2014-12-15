using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.AspNetMvc.Services;

namespace Atomia.Store.Services.Fakes
{
    public class FakeResourceProvider : IResourceProvider
    {
        public string GetResource(string resourceName)
        {
            return "Resource string for " + resourceName;
        }
    }
}
