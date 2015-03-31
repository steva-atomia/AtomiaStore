using Atomia.Store.PublicBillingApi.Handlers;
using System;
using System.Collections.Generic;

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
            get { return "CsDomainParking"; }
        }

        /// <summary>
        /// Use "CsLinuxWebsite" for connected packages that are allowed to have website, otherwise the default "CsDomainParking"
        /// </summary>
        protected override string GetAtomiaService(ItemData connectedItem)
        {
            if (IsHostingPackageWithWebsitesAllowed(connectedItem))
            {
                return "CsLinuxWebsite";
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
