using Atomia.Store.PublicBillingApi.Handlers;
using System;
using System.Collections.Generic;

namespace Atomia.Store.PublicOrderHandlers.CartItemHandlers
{
    public class RegisterDomainHandler : BaseDomainHandler
    {
        public override IEnumerable<string> HandledCategories
        {
            get { return new[] { "TLD" }; }
        }

        public override string DefaultAtomiaService
        {
            get { return "CsDomainParking"; }
        }

        protected override string GetAtomiaService(ItemData connectedItem)
        {
            if (IsHostingPackageWithWebsitesAllowed(connectedItem))
            {
                return "CsLinuxWebsite";
            }

            return DefaultAtomiaService;
        }

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
