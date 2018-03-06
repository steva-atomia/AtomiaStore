using Atomia.Store.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Atomia.Web.Plugin.DomainSearch.Helpers;

namespace Atomia.Store.Themes.Default.Adapters
{
    /// <summary>
    /// Decorates a base domains provider by setting Premium custom attribute on selected TLDs
    /// </summary>
    public sealed class PremiumDomainsProvider : IDomainsProvider
    {
        private readonly IDomainsProvider domainsProvider;
        private readonly string sessionPrefix = "PremiumDomainsProvider";

        /// <summary>
        /// Constructor that takes the base <see cref="Atomia.Store.Core.IDomainsProvider"/> as dependency.
        /// </summary>
        /// <param name="domainsProvider"></param>
        public PremiumDomainsProvider(IDomainsProvider domainsProvider)
        {
            if (domainsProvider == null)
            {
                throw new ArgumentNullException("domainsProvider");
            }

            this.domainsProvider = domainsProvider;
        }

        /// <summary>
        /// Find domains and add "Premium" custom attribute to selected results.
        /// </summary>
        public DomainSearchData FindDomains(ICollection<string> searchTerms)
        {
            var data = domainsProvider.FindDomains(searchTerms);

            var tlds = DomainSearchHelper.StripProtocolFromDomainNames(searchTerms.ToArray());
            data.Results = AddPremiumCustomAttribute(data.Results, tlds);

            System.Web.HttpContext.Current.Session[sessionPrefix + data.DomainSearchId] = searchTerms;

            return data;
        }

        /// <summary>
        /// Check availablility status and add "Premium" custom attribute to selected results.
        /// </summary>
        public DomainSearchData CheckStatus(int domainSearchId)
        {
            var data = domainsProvider.CheckStatus(domainSearchId);
            var searchTerms = System.Web.HttpContext.Current.Session[sessionPrefix + data.DomainSearchId] as ICollection<string>;

            var tlds = new List<string>();
            if(searchTerms != null)
            {
                tlds = DomainSearchHelper.StripProtocolFromDomainNames(searchTerms.ToArray());
            }

            data.Results = AddPremiumCustomAttribute(data.Results, tlds);

            return data;
        }

        /// <summary>
        /// Get the domain categories from the base domains provider.
        /// </summary>
        public IEnumerable<string> GetDomainCategories()
        {
            return domainsProvider.GetDomainCategories();
        }

        /// <summary>
        /// Add "Premium" custom attribute to TLDs listed in the "PremiumTLDs" app setting.
        /// </summary>
        private IEnumerable<DomainResult> AddPremiumCustomAttribute(IEnumerable<DomainResult> domains, List<string> tlds)
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

            tlds = tlds.Where(t => !premiumTlds.Any(p => p == t)).ToList();
            premiumTlds.AddRange(tlds);

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