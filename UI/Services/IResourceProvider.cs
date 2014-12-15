using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.AspNetMvc.Services
{
    public interface IResourceProvider
    {
        string GetResource(string resourceName);
    }
}
