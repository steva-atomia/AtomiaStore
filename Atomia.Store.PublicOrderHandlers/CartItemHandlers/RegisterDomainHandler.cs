using Atomia.Store.PublicBillingApi.Handlers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Atomia.Store.PublicOrderHandlers.CartItemHandlers
{
    public class RegisterDomainHandler : BaseDomainHandler
    {
        /// <summary>
        /// Handle items with category "TLD"
        /// </summary>
        public override IEnumerable<string> HandledCategories
        {
            get { return new[] { "TLD" }; }
        }

        /// <summary>
        /// Use "CsDomainParking" service by default for packages with same domain name as handled TLD item.
        /// </summary>
        public override string DefaultAtomiaService
        {
            get {
                var atomiaService = Atomia.Web.Base.Helpers.General.PluginSettingsHelper.FetchSetting("DefaultDNSService",
                                        Assembly.GetExecutingAssembly().CodeBase);
                return string.IsNullOrEmpty(atomiaService) ? "CsDomainParking" : atomiaService;
            }
        }

        /// <summary>
        /// Use "CsLinuxWebsite" for connected packages that are allowed to have website, otherwise the default "CsDomainParking"
        /// </summary>
        protected override string GetAtomiaService(ItemData connectedItem)
        {
            if (IsHostingPackageWithWebsitesAllowed(connectedItem))
            {
                var atomiaService = Atomia.Web.Base.Helpers.General.PluginSettingsHelper.FetchSetting("DefaultHostingService",
                                        Assembly.GetExecutingAssembly().CodeBase);
                return string.IsNullOrEmpty(atomiaService) ? "CsLinuxWebsite" : atomiaService;
            }

            return DefaultAtomiaService;
        }

        /// <summary>
        /// Add ExtraServiceProperties from HostingPackages that are allowed to have website provisioned.
        /// </summary>
        protected override string GetAtomiaServiceExtraProperties(ItemData connectedItem)
        {
            if (IsHostingPackageWithWebsitesAllowed(connectedItem))
            {
                return base.GetAtomiaServiceExtraProperties(connectedItem);
            }

            return String.Empty;
        }
    }
}
