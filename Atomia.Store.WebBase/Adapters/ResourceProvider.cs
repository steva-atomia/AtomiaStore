using Atomia.Store.Core;
using System;
using System.Web.Mvc;

namespace Atomia.Store.WebBase.Adapters
{
    public sealed class ResourceProvider : IResourceProvider
    {
        private readonly IThemeNamesProvider themeNamesProvider = DependencyResolver.Current.GetService<IThemeNamesProvider>();

        public string GetResource(string resourceName)
        {
            var themeClass = themeNamesProvider.GetMainThemeName() + "Common,";
            var resource = LocalizationHelpers.GlobalResource(themeClass + resourceName);

            return String.IsNullOrEmpty(resource)
                ? LocalizationHelpers.GlobalResource("Common," + resourceName)
                : resource;
        }
    }
}
