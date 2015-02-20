using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.PublicOrderHandlers.CartItemHandlers
{
    public abstract class BaseDomainHandler : OrderDataHandler
    {
        public abstract string DefaultAtomiaService { get; }

        public abstract IEnumerable<string> HandledCategories { get; }

        public override PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext)
        {
            var domainItems = orderContext.ItemData.Where(i => this.HandledCategories.Contains(i.Category));

            foreach (var domainItem in domainItems)
            {
                var customData = new List<PublicOrderItemProperty>();

                var domainName = GetDomainName(domainItem);
                var connectedItem = GetConnectedItem(orderContext.ItemData, domainItem, domainName);
                
                customData.Add(new PublicOrderItemProperty { Name = "DomainName", Value = domainName });

                if (connectedItem != null)
                {
                    var atomiaService = GetAtomiaService(connectedItem);
                    customData.Add(new PublicOrderItemProperty { Name = "AtomiaService", Value = atomiaService });

                    var extraProperties = GetAtomiaServiceExtraProperties(domainItem);
                    if (!String.IsNullOrEmpty(extraProperties))
                    {
                        customData.Add(new PublicOrderItemProperty
                        {
                            Name = "AtomiaServiceExtraProperties",
                            Value = extraProperties
                        });
                    }
                }
                else
                {
                    customData.Add(new PublicOrderItemProperty { Name = "AtomiaService", Value = DefaultAtomiaService });
                }

                foreach(var extraData in GetExtraCustomData(domainItem))
                {
                    customData.Add(extraData);
                }

                var orderItem = new PublicOrderItem
                {
                    ItemId = Guid.Empty,
                    ItemNumber = domainItem.ArticleNumber,
                    Quantity = 1,
                    RenewalPeriodId = domainItem.RenewalPeriodId,
                    CustomData = customData.ToArray()
                };

                Add(order, orderItem);
            }

            return order;
        }

        protected virtual string GetDomainName(ItemData domainItem)
        {
            var domainNameAttr = domainItem.CartItem.CustomAttributes.FirstOrDefault(ca => ca.Name == "DomainName");

            if (domainNameAttr == null)
            {
                throw new InvalidOperationException("Domain registration cart item must have CustomAttribute \"DomainName\".");
            }

            var domainName = Normalize(domainNameAttr.Value);

            return domainName;
        }

        protected virtual ItemData GetConnectedItem(IEnumerable<ItemData> allItems, ItemData domainItem, string domainName)
        {
            var connectedItem = allItems.FirstOrDefault(other =>
                    other.ArticleNumber != domainItem.ArticleNumber &&
                    other.CartItem.CustomAttributes.Any(ca => ca.Name == "DomainName" && ca.Value == domainName));

            return connectedItem;
        }


        protected virtual string GetAtomiaService(ItemData connectedItem)
        {
            return DefaultAtomiaService;
        }

        protected virtual string GetAtomiaServiceExtraProperties(ItemData connectedItem)
        {
            var extraProperties = String.Empty;

            if (connectedItem != null)
            {
                var extraPropertiesAttr = connectedItem.Product.CustomAttributes.FirstOrDefault(ca => ca.Name == "atomiaserviceextraproperties");
                if (extraPropertiesAttr != null)
                {
                    extraProperties = extraPropertiesAttr.Value;
                }
            }

            return extraProperties;
        }

        protected virtual IEnumerable<PublicOrderItemProperty> GetExtraCustomData(ItemData domainItem)
        {
            return new List<PublicOrderItemProperty>();
        }

        protected bool IsHostingPackageWithWebsitesAllowed(ItemData connectedItem)
        {
            return (connectedItem.Category == "HostingPackage" &&
                    !connectedItem.Product.CustomAttributes.Any(ca => ca.Name == "nowebsites" && ca.Value.ToLowerInvariant() == "true"));
        }
    }
}
