using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Atomia.Store.Core;
using Atomia.Web.Plugin.DomainSearch.Models;
using Atomia.Web.Plugin.DomainSearch.Helpers;
using SimpleDnsPlus;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    public class DomainsProvider : PublicBillingApiClient, IDomainsProvider
    {
        private readonly IProductProvider productProvider;
        private readonly Guid resellerId;
        private readonly string currencyCode;
        private readonly string countryCode;

        public DomainsProvider(
            ResellerDataProvider resellerDataProvider, 
            ICurrencyPreferenceProvider currencyPreferenceProvider, 
            IProductProvider productProvider, 
            PublicBillingApiProxy billingApi) : base(billingApi)
        {
            if (resellerDataProvider == null)
            {
                throw new ArgumentException("resellerDataProvider");
            }

            if (currencyPreferenceProvider == null)
            {
                throw new ArgumentException("currencyPreferenceProvider");
            }

            if (productProvider == null)
            {
                throw new ArgumentNullException("productProvider");
            }

            var resellerData = resellerDataProvider.GetResellerAccountData();

            this.resellerId = resellerData.Id;
            this.countryCode = resellerData.DefaultCountry.Code;
            this.currencyCode = currencyPreferenceProvider.GetCurrentCurrency().Code;
            this.productProvider = productProvider;
        }

        public DomainSearchData FindDomains(ICollection<string> searchTerms)
        {
            var results = new List<DomainResult>();

            var domainNames = GetDomainNames(searchTerms);
            var unavailableDomains = CheckLocalStatus(domainNames);
            var domainNamesToCheck = GetDomainNamesToCheck(domainNames, unavailableDomains);
            var checkedDomains = StartSearch(domainNamesToCheck);

            foreach (var domain in checkedDomains.Concat(unavailableDomains))
            {
                var domainResult = CreateDomainResult(domain.ProductID, domain.ProductStatus, domain.ProductName, domain.TransactionId);
                results.Add(domainResult);
            }

            var firstCheckedDomain = checkedDomains.FirstOrDefault();
            var domainSearchId = firstCheckedDomain != null ? firstCheckedDomain.TransactionId : -1;

            var data = new DomainSearchData
            {
                FinishSearch = !results.Any(r => r.Status == DomainResult.LOADING),
                DomainSearchId = domainSearchId,
                Results = results.OrderBy(d => d.TLD)
            };

            return data;
        }

        public DomainSearchData CheckStatus(int domainSearchId)
        {
            var results = new List<DomainResult>();

            var statusData = DomainSearchHelper.GetAvailabilityStatus(
                domainSearchId.ToString(),
                BillingApi.Service,
                Guid.Empty,
                resellerId,
                currencyCode,
                countryCode);
            
            int transactionId;
            if (!Int32.TryParse(statusData.TransactionId, out transactionId))
            {
                transactionId = -1;
            };

            foreach(var domain in statusData.DomainStatuses)
            {
                var domainResult = CreateDomainResult(domain.ProductId, domain.Status, domain.DomainName, transactionId);
                results.Add(domainResult);
            }

            var data = new DomainSearchData
            {
                DomainSearchId = transactionId,
                FinishSearch = statusData.FinishSearch,
                Results = results.OrderBy(d => d.TLD)
            };

            return data;
        }

        public IEnumerable<string> GetDomainCategories()
        {
            return new List<string> { "TLD", "TransferTLD", "OwnDomain" };
        }

        /// <summary>
        /// Gets a list of domain names based on tld:s available for sale.
        /// </summary>
        private IEnumerable<string> GetDomainNames(ICollection<string> searchTerms)
        {
            var domainsArray = String.Join(" ", searchTerms);
            var domainsForCheck = DomainSearchHelper.GetDomainsForCheck(domainsArray, this.resellerId);

            return domainsForCheck;
        }

        /// <summary>
        /// Gets a list of domains that are either taken locally or failing to match tld requirements.
        /// </summary>
        private IEnumerable<DomainDataFromXml> CheckLocalStatus(IEnumerable<string> domains)
        {
            var results = new List<DomainDataFromXml>();
            var localStatusString = String.Empty;
            var domainCheckAttributes = BillingApi.CheckDomains(domains);
            var tldBasedRegexStrings = DomainSearchHelper.GetTLDBasedRegexes(this.resellerId);
            IEnumerable<Regex> tldBasedRegexes = null;

            if (tldBasedRegexStrings.Count > 0)
            {
                tldBasedRegexes = tldBasedRegexStrings.Select(r => new Regex(r));
            }

            foreach (var attr in domainCheckAttributes)
            {
                if (attr.Value.ToLower() == "taken")
                {
                    localStatusString += attr.Name + "|TAKEN ";
                }
                else if (tldBasedRegexes != null)
                {
                    var decodedDomainName = IDNLib.Decode(attr.Name.Trim());

                    if (!tldBasedRegexes.Any(regex => regex.IsMatch(decodedDomainName)))
                    {
                        localStatusString += attr.Name + "|SPECIAL ";
                    }
                }
            }
            
            if (localStatusString != String.Empty)
            {
                results = DomainSearchHelper.MarkDomainsAsUnavailable(
                    localStatusString.TrimEnd(' '),
                    BillingApi.Service,
                    Guid.Empty,
                    this.resellerId,
                    this.currencyCode,
                    this.countryCode).ToList();

                foreach (var domainData in results)
                {
                    domainData.ProductName = IDNLib.Encode(domainData.ProductName);
                }
            }

            return results;
        }

        private IEnumerable<string> GetDomainNamesToCheck(IEnumerable<string> domainNames, IEnumerable<DomainDataFromXml> alreadyCheckedDomains)
        {
            var domainNamesToCheck = new HashSet<string>(domainNames);
            domainNamesToCheck.ExceptWith(alreadyCheckedDomains.Select(d => d.ProductName));

            return domainNamesToCheck;
        }

        private IEnumerable<DomainDataFromXml> StartSearch(IEnumerable<string> domainNamesToCheck)
        {
            var result = DomainSearchHelper.StartSearch(
                domainNamesToCheck.ToArray(),
                BillingApi.Service,
                Guid.Empty,
                this.resellerId,
                this.currencyCode,
                this.countryCode);

            return result;
        }

        private DomainResult CreateDomainResult(string productId, string productStatus, string productName, int transactionId)
        {
            var product = productProvider.GetProduct(productId);
            var status = DomainResult.UNKNOWN;

            switch (productStatus.ToLower())
            {
                case ("processing"):
                case ("loading"):
                    status = DomainResult.LOADING;
                    break;
                case ("taken"):
                case ("special"):
                    status = DomainResult.UNAVAILABLE;
                    break;
                case ("available"):
                    status = DomainResult.AVAILABLE;
                    break;
                case ("warning"):
                default:
                    status = DomainResult.UNKNOWN;
                    break;
            }

            var tldAttr = product.CustomAttributes.FirstOrDefault(ca => ca.Name == "productvalue");

            if (tldAttr == null)
            {
                throw new InvalidOperationException(String.Format("product {0} is missing required 'productvalue' custom attribute", product.ArticleNumber));
            }

            var tld = tldAttr.Value.ToLower().TrimStart('.');

            var domainResult = new DomainResult(product, tld, productName, status, transactionId);

            return domainResult;
        }
    }
}
