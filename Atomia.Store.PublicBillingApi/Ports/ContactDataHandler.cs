using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.PublicBillingApi.Ports
{
    public abstract class ContactDataHandler
    {
        public abstract string Id { get; }

        /// <summary>
        /// Amend order with contact data
        /// </summary>
        /// <param name="order">The order to amend.</param>
        public virtual void AmendOrder(ContactData paymentMethodData, PublicOrder order, List<PublicOrderCustomData> customData)
        {
        }
    }
}
