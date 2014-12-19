using Atomia.Store.Core;
using System.Web.Mvc;

namespace Atomia.Store.WebBase
{
    public class ResourceProvider : IResourceProvider
    {
        public string GetResource(string resourceName)
        {
            return LocalizationHelpers.GlobalResource("Common," + resourceName);
        }
    }
}
