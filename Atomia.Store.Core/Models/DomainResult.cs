using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Core
{
    /// <summary>
    /// A domain search result
    /// </summary>
    public sealed class DomainResult
    {
        public const string AVAILABLE = "available";

        public const string UNAVAILABLE = "unavailable";

        public const string UNKNOWN = "unknown";

        public const string LOADING = "loading";

        private Product product;
        private string domainName;
        private string status;
        private int domainSearchId;
        private string tld;

        /// <summary>
        /// Domain result constructor
        /// </summary>
        /// <param name="product">The underlying <see cref="Product"/></param>
        /// <param name="tld">The TLD</param>
        /// <param name="domainName">The domain name</param>
        /// <param name="status">The status, see constants above.</param>
        /// <param name="domainSearchId">The id of the domain search that yielded this result.</param>
        public DomainResult(Product product, string tld, string domainName, string status, int domainSearchId)
        {
            this.product = product;

            if (product.CustomAttributes == null)
            {
                product.CustomAttributes = new List<CustomAttribute>();
            }

            this.status = status;
            this.domainSearchId = domainSearchId;
            this.tld = tld;
            this.DomainName = domainName;
        }

        /// <summary>
        /// The underlying <see cref="Product"/>
        /// </summary>
        public Product Product
        {
            get
            {
                return product;
            }
        }

        /// <summary>
        /// The id of the domain search that yielded this result.
        /// </summary>
        public int DomainSearchId
        {
            get
            {
                return domainSearchId;
            }
        }

        /// <summary>
        /// The availability status: <see cref="AVAILABLE"/>, <see cref="UNAVAILABLE"/>, <see cref="UNKNOWN"/> or <see cref="LOADING"/>
        /// </summary>
        public string Status
        {
            get
            {
                return status;
            }
        }

        /// <summary>
        /// The TLD this result belongs to. No dot.
        /// </summary>
        /// <example>"com", "net", "org"</example>
        public string TLD
        {
            get
            {
                return tld;
            }
        }

        public int Order { get; set; }

        /// <summary>
        /// The domain name, including TLD.
        /// </summary>
        /// <example>"example.com"</example>
        public string DomainName {
            get
            {
                return domainName;
            }
            set
            {
                var domainNameAttr = Product.CustomAttributes.FirstOrDefault(ca => ca.Name == "DomainName");
                
                if (domainNameAttr != null)
                {
                    domainNameAttr.Value = value;
                }
                else
                {
                    Product.CustomAttributes.Add(new CustomAttribute { Name = "DomainName", Value = value });
                }

                domainName = value;
            }
        }
    }
}
