using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Providers
{
    public class PremiumDomainsProvider : DomainsProvider
    {
        protected readonly DomainSearchProvider domainSearchProvider;

        public PremiumDomainsProvider(DomainSearchProvider domainSearchProvider)
        {
            this.domainSearchProvider = domainSearchProvider;
        }

        public override IEnumerable<Product> GetProducts(ProductSearchQuery searchQuery)
        {
            var domains = domainSearchProvider.GetProducts(searchQuery);
            var premiumTlds = new List<string>();

            var premiumTldsConfig = ConfigurationManager.AppSettings["PremiumDomainArticleNumbers"];
            if (!string.IsNullOrEmpty(premiumTldsConfig))
            {
                premiumTlds = premiumTldsConfig.Split(',').ToList();
            }

            foreach(var domain in domains)
            {
                if (premiumTlds.Any(x => x.Trim().ToLowerInvariant() == domain.ArticleNumber.ToLowerInvariant()))
                {
                    domain.CustomAttributes.Add(new CustomAttribute
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
