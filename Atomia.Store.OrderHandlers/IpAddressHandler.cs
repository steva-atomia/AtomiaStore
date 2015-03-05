using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Linq;
using System.Web;

namespace Atomia.Store.PublicOrderHandlers
{
    public class IpAddressHandler : OrderDataHandler
    {
        public override PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext)
        {
            var request = orderContext.ExtraData.OfType<HttpRequestBase>().FirstOrDefault();

            if (request != null)
            {
                var ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!String.IsNullOrEmpty(ip))
                {
                    var ipRange = ip.Split(',');
                    var trueIp = ipRange[0];

                    Add(order, new PublicOrderCustomData { Name = "IpAddress", Value = trueIp });
                }
                else
                {
                    ip = request.ServerVariables["REMOTE_ADDR"];
                    Add(order, new PublicOrderCustomData { Name = "IpAddress", Value = ip });
                }
            }

            return order;
        }
    }
}
