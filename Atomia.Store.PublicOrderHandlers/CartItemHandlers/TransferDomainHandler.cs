using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System.Collections.Generic;

namespace Atomia.Store.PublicOrderHandlers.CartItemHandlers
{
    public class TransferDomainHandler : BaseDomainHandler
    {
        /// <summary>
        /// Handle items with category "TransferTLD"
        /// </summary>
        public override IEnumerable<string> HandledCategories
        {
            get { return new[] { "TransferTLD" }; }
        }

        /// <summary>
        /// Use "CsDomainParking" service by default for packages with same domain name as handled TransferTLD item.
        /// </summary>
        public override string DefaultAtomiaService
        {
            get { return "CsDomainParking"; }
        }

        /// <summary>
        /// Get domain name with any "www" prefix removed
        /// </summary>
        protected override string GetDomainName(ItemData domainItem)
        {
            var baseDomainName =  base.GetDomainName(domainItem);
            var domainName = baseDomainName.StartsWith("www")
                ? baseDomainName.Remove(0, 4)
                : baseDomainName;

            return domainName;
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
        /// Add custom attribute with authcode
        /// </summary>
        protected override IEnumerable<PublicOrderItemProperty> GetExtraCustomData(ItemData domainItem)
        {
            // TODO: Add authcode
            return base.GetExtraCustomData(domainItem);
        }
    }
}
