using System;
using System.Collections.Generic;
using System.Linq;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.PublicBillingApi
{
    /// <summary>
    /// Proxy AtomiaBillingPublicService to be able to override for unit tests and such.
    /// </summary>
    public class PublicBillingApiProxy
    {
        private AtomiaBillingPublicService service;

        public PublicBillingApiProxy(AtomiaBillingPublicService service)
        {
            this.service = service;
        }

        public AtomiaBillingPublicService Service
        {
            get
            {
                return service;
            }
        }

        public virtual PublicOrder CalculateOrder(PublicOrder publicOrder)
        {
            return service.CalculateOrder(publicOrder);
        }

        public virtual IEnumerable<Country> GetCountries()
        {
            return service.GetCountries();
        }

        public AccountData GetAccountDataByHash(string accountHash)
        {
            return service.GetAccountDataByHash(accountHash);
        }

        public AccountData GetResellerDataByUrl(string orderPageUrl)
        {
            return service.GetResellerDataByUrl(orderPageUrl);
        }

        public AccountData GetDefaultResellerData()
        {
            return service.GetDefaultResellerData();
        }

        public IEnumerable<AttributeData> CheckDomains(IEnumerable<string> domains)
        {
            var domainsArray = domains.ToArray();
            
            return service.CheckDomains(domainsArray);
        }
    }
}
