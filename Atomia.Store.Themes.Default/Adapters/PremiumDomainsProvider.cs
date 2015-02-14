using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Atomia.Store.Core;

namespace Atomia.Store.Themes.Default.Adapters
{
    /// <summary>
    /// Decorates a base provider by setting Premium custom attribute on selected TLDs
    /// </summary>
    public class PremiumDomainsProvider : IDomainsProvider
    {
        private readonly IDomainsProvider domainsProvider;

        public PremiumDomainsProvider(IDomainsProvider domainsProvider)
        {
            if (domainsProvider == null)
            {
                throw new ArgumentNullException("domainsProvider");
            }

            this.domainsProvider = domainsProvider;
        }

        public DomainSearchData FindDomains(ICollection<string> searchTerms)
        {
            var data = domainsProvider.FindDomains(searchTerms);
            data.Results = AddPremiumCustomAttribute(data.Results);

            return data;
        }

        public DomainSearchData CheckStatus(int domainSearchId)
        {
            var data = domainsProvider.CheckStatus(domainSearchId);
            data.Results = AddPremiumCustomAttribute(data.Results);

            return data;
        }

        public IEnumerable<string> GetDomainCategories()
        {
            return domainsProvider.GetDomainCategories();
        }

        private IEnumerable<DomainResult> AddPremiumCustomAttribute(IEnumerable<DomainResult> domains)
        {
            var premiumTldsSetting = ConfigurationManager.AppSettings["PremiumTLDs"] as String;
            var premiumTlds = new List<string>();

            if (!String.IsNullOrEmpty(premiumTldsSetting))
            {
                premiumTlds = premiumTldsSetting
                    .Split(',')
                    .Select(t => t.Trim())
                    .ToList();
            }

            foreach (var domain in domains)
            {
                var product = domain.Product;
                var tld = domain.TLD;

                if (premiumTlds.Contains(tld))
                {
                    product.CustomAttributes.Add(new CustomAttribute
                    {
                        Name = "Premium",
                        Value = "true"
                    });
                }
            }

            return domains;
        }
    }
}