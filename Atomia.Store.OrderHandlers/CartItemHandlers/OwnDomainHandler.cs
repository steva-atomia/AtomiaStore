using Atomia.Store.PublicBillingApi.Handlers;
using System.Collections.Generic;

namespace Atomia.Store.PublicOrderHandlers.CartItemHandlers
{
    public class OwnDomainHandler : BaseDomainHandler
    {
        public override IEnumerable<string> HandledCategories
        {
            get { return new[] { "OwnDomain" }; }
        }

        public override string DefaultAtomiaService
        {
            get { return "CsDomainParking"; }
        }

        protected override string GetDomainName(ItemData domainItem)
        {
            var baseDomainName = base.GetDomainName(domainItem);
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
    }
}
