using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.PublicBillingApi;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

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

        protected override bool SetMainDomain(ItemData connectedItem)
        {
            return IsHostingPackageWithWebsitesAllowed(connectedItem);
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
