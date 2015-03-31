using Atomia.Store.Core;
using System;
using System.Web.Mvc;

namespace Atomia.Store.WebBase.Adapters
{
    /// <summary>
    /// <see cref="Atomia.Store.Core.IResourceProvider"/> that reads only from Common.resx in App_GlobalResources 
    /// using  localization extensions from Atomia.Web.Base
    /// </summary>
    public sealed class ResourceProvider : IResourceProvider
    {
        private readonly IThemeNamesProvider themeNamesProvider = DependencyResolver.Current.GetService<IThemeNamesProvider>();

        /// <inheritdoc />
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
