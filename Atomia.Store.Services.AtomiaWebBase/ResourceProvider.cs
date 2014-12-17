using Atomia.Store.AspNetMvc.Services;
using System.Web.Mvc;

namespace Atomia.Store.Services.WebBase
{
    public class ResourceProvider : IResourceProvider
    {
        public string GetResource(string resourceName)
        {
            return LocalizationHelpers.GlobalResource("Common," + resourceName);
        }
    }
}
