using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System.Collections.Generic;

namespace Atomia.Store.PublicOrderHandlers.CartItemHandlers
{
    public class TransferDomainHandler : BaseDomainHandler
    {
        public override IEnumerable<string> HandledCategories
        {
            get { return new[] { "TransferTLD" }; }
        }

        public override string DefaultAtomiaService
        {
            get { return "CsDomainParking"; }
        }

        protected override string GetDomainName(ItemData domainItem)
        {
            var baseDomainName =  base.GetDomainName(domainItem);
            var domainName = baseDomainName.StartsWith("www")
                ? baseDomainName.Remove(0, 4)
                : baseDomainName;

            return domainName;
        }
        protected override string GetAtomiaService(ItemData connectedItem)
        {
            if (IsHostingPackageWithWebsitesAllowed(connectedItem))
            {
                return "CsLinuxWebsite";
            }

            return DefaultAtomiaService;
        }

        protected override IEnumerable<PublicOrderItemProperty> GetExtraCustomData(ItemData domainItem)
        {
            // TODO: Add authcode
            return base.GetExtraCustomData(domainItem);
        }
    }
}
