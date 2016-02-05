using Atomia.Store.Core;
using Atomia.Web.Plugin.DomainSearch.Helpers;
using Atomia.Web.Plugin.DomainSearch.Models;
using SimpleDnsPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ApiProduct = Atomia.Web.Plugin.ProductsProvider.Product;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    /// <summary>
    /// Domain names provider for Atomia Billing and Atomia DomainRegistration via Atomia.Web.Plugin.DomainSearch plugin.
    /// Returns exact matches of search terms for TLDs that are available for current reseller.
    /// </summary>
    public sealed class DomainsProvider : PublicBillingApiClient, IDomainsProvider
    {
        private readonly Guid resellerId;
        private readonly string currencyCode;
        private readonly string countryCode;
        private readonly IList<ApiProduct> tldProducts;
        private readonly ProductMapper productMapper;
        private readonly IShopNameProvider shopNameProvider;

        /// <summary>
        /// Construct a new instance
        /// </summary>
        public DomainsProvider(
            IResellerDataProvider resellerDataProvider, 
            ICurrencyPreferenceProvider currencyPreferenceProvider, 
            ApiProductsProvider apiProductsProvider, 
            ProductMapper productMapper,
            PublicBillingApiProxy billingApi,
            IShopNameProvider shopNameProvider) : base(billingApi)
        {
            if (resellerDataProvider == null)
            {
                throw new ArgumentException("resellerDataProvider");
            }

            if (currencyPreferenceProvider == null)
            {
                throw new ArgumentException("currencyPreferenceProvider");
            }

            if (apiProductsProvider == null)
            {
                throw new ArgumentNullException("apiProductsProvider");
            }

            if (productMapper == null)
            {
                throw new ArgumentNullException("productMapper");
            }

            if (shopNameProvider == null)
            {
                throw new ArgumentNullException("shopNameProvider");
            }

            var resellerData = resellerDataProvider.GetResellerAccountData();

            this.resellerId = resellerData.Id;
            this.countryCode = resellerData.DefaultCountry.Code;
            this.currencyCode = currencyPreferenceProvider.GetCurrentCurrency().Code;
            this.productMapper = productMapper;
            this.shopNameProvider = shopNameProvider;

            this.tldProducts = apiProductsProvider.GetProductsByCategories(new List<string> { "TLD" });
        }

        /// <summary>
        /// Find domain names that match search terms.
        /// </summary>
        /// <param name="searchTerms">A list of search terms</param>
        /// <returns>A list of domain names that match the search terms</returns>
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
                Results = results
            };

            return data;
        }

        /// <summary>
        /// Check status of results from search with specified id
        /// </summary>
        public DomainSearchData CheckStatus(int domainSearchId)
        {
            var results = new List<DomainResult>();

            var statusData = DomainSearchHelper.GetAvailabilityStatus(
                domainSearchId.ToString(),
                BillingApi.Service,
                Guid.Empty,
                resellerId,
                this.shopNameProvider.GetShopName(),
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
                Results = results
            };

            return data;
        }

        /// <summary>
        /// Get default domain categories from Atomia Billing: TLD, TransferTLD, OwnDomain
        /// </summary>
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
            var domainsForCheck = DomainSearchHelper.GetDomainsForCheck(
                domainsArray,
                this.resellerId,
                Guid.Empty,
                this.shopNameProvider.GetShopName(),
                this.countryCode);

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
            var tldBasedRegexStrings = DomainSearchHelper.GetTLDBasedRegexes(
                this.resellerId,
                this.shopNameProvider.GetShopName(),
                this.countryCode);
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
                    this.shopNameProvider.GetShopName(),
                    this.currencyCode,
                    this.countryCode).ToList();

                foreach (var domainData in results)
                {
                    domainData.ProductName = IDNLib.Encode(domainData.ProductName);
                }
            }

            return results;
        }

        /// <summary>
        /// Filter domain names to check with Atomia DomainRegistration from those already checked locally.
        /// </summary>
        /// <param name="domainNames">All domain names in the search</param>
        /// <param name="alreadyCheckedDomains">The domain names that have already been checked locally</param>
        /// <returns>Domain names to check with Atomia DomainRegistration</returns>
        private IEnumerable<string> GetDomainNamesToCheck(IEnumerable<string> domainNames, IEnumerable<DomainDataFromXml> alreadyCheckedDomains)
        {
            var domainNamesToCheck = new HashSet<string>(domainNames);
            domainNamesToCheck.ExceptWith(alreadyCheckedDomains.Select(d => d.ProductName));

            return domainNamesToCheck;
        }

        /// <summary>
        /// Start search for domain names in Atomia DomainRegistration
        /// </summary>
        private IEnumerable<DomainDataFromXml> StartSearch(IEnumerable<string> domainNamesToCheck)
        {
            if (domainNamesToCheck == null || domainNamesToCheck.Count() <= 0)
            {
                return new List<DomainDataFromXml>();
            }

            var result = DomainSearchHelper.StartSearch(
                domainNamesToCheck.ToArray(),
                BillingApi.Service,
                Guid.Empty,
                this.resellerId,
                this.shopNameProvider.GetShopName(),
                this.currencyCode,
                this.countryCode);

            return result;
        }

        /// <summary>
        /// Convert data from DomainSearch plugin to AtomiaStore native <see cref="Atomia.Store.Core.DomainResult"/>
        /// </summary>
        /// <param name="productId">Article number of the domain registration product</param>
        /// <param name="productStatus">Domain name availability status</param>
        /// <param name="productName">Name of the domain registration product</param>
        /// <param name="transactionId">Id of the search that the result is from</param>
        /// <returns>Single domain result</returns>
        private DomainResult CreateDomainResult(string productId, string productStatus, string productName, int transactionId)
        {
            var apiProduct = this.tldProducts.FirstOrDefault(p => p.ArticleNumber == productId);
            if (apiProduct == null)
            {
                throw new InvalidOperationException(string.Format("No TLD product with articlenumber {0}", productId));
            }

            string productvalue;
            if (!apiProduct.Properties.TryGetValue("productvalue", out productvalue))
            {
                throw new InvalidOperationException(String.Format("product {0} is missing required 'productvalue' property", productId));
            }

            var status = DomainResult.UNKNOWN;

            switch (productStatus.ToLower())
            {
                case ("processing"):
                case ("loading"):
                    status = DomainResult.LOADING;
                    break;
                case ("taken"):
                case ("unavailable"):
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

            var product = productMapper.Map(apiProduct);
            var tld = productvalue.ToLower().TrimStart('.');

            var domainResult = new DomainResult(product, tld, productName, status, transactionId);

            // This makes sure results get the same Order property independent of if they are from initial search or status check.
            // Since tldProducts come directly from API they are sorted in that order.
            domainResult.Order = tldProducts.IndexOf(apiProduct);

            return domainResult;
        }
    }
}
