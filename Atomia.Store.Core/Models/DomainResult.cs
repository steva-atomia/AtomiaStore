using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Core
{
    public class DomainResult
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

        public Product Product
        {
            get
            {
                return product;
            }
        }

        public int DomainSearchId
        {
            get
            {
                return domainSearchId;
            }
        }

        public string Status
        {
            get
            {
                return status;
            }
        }

        public string TLD
        {
            get
            {
                return tld;
            }
        }

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
