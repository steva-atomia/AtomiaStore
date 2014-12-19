using Atomia.Store.Core;
using System.Web.Mvc;

namespace Atomia.Store.WebBase.Adapters
{
    public class ResourceProvider : IResourceProvider
    {
        public string GetResource(string resourceName)
        {
            return LocalizationHelpers.GlobalResource("Common," + resourceName);
        }
    }
}
