using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// A specialization of <see cref="ProductModel"/> for <see cref="Atomia.Store.Core.DomainResult">DomainResults</see>
    /// </summary>
    public class DomainResultModel : ProductModel
    {
        private readonly string domainName;
        private readonly string status;
        private readonly int domainSearchId;
        private readonly int order;
        
        /// <summary>
        /// Construct the view model from the <see cref="DomainResult"/>
        /// </summary>
        public DomainResultModel(DomainResult domainResult)
            : base(domainResult.Product)
        {
            domainName = domainResult.DomainName;
            status = domainResult.Status;
            domainSearchId = domainResult.DomainSearchId;
            order = domainResult.Order;
        }

        /// <summary>
        /// The domain name from the <see cref="Atomia.Store.Core.DomainResult"/>
        /// </summary>
        public string DomainName
        {
            get
            {
                return domainName;
            }
        }

        /// <summary>
        /// The availability status from the <see cref="Atomia.Store.Core.DomainResult"/>
        /// </summary>
        public string Status
        {
            get
            {
                return status;
            }
        }

        /// <summary>
        /// The id of the domain search the <see cref="Atomia.Store.Core.DomainResult"/> is related to.
        /// </summary>
        public int DomainSearchId
        {
            get
            {
                return domainSearchId;
            }
        }

        public int Order
        {
            get
            {
                return order;
            }
        }
    }
}
