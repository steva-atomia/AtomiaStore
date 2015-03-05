using Atomia.Store.Core;
using System.Web.Mvc;

namespace Atomia.Store.WebBase.Adapters
{
    public sealed class ResourceProvider : IResourceProvider
    {
        public string GetResource(string resourceName)
        {
            return LocalizationHelpers.GlobalResource("Common," + resourceName);
        }
    }
}
