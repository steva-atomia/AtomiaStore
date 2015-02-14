using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Models
{
    public class DomainResultModel : ProductModel
    {
        private readonly string domainName;
        private readonly string status;
        private readonly int domainSearchId;
        
        public DomainResultModel(DomainResult domainResult)
            : base(domainResult.Product)
        {
            domainName = domainResult.DomainName;
            status = domainResult.Status;
            domainSearchId = domainResult.DomainSearchId;
        }

        public string DomainName
        {
            get
            {
                return domainName;
            }
        }

        public string Status
        {
            get
            {
                return status;
            }
        }

        public int DomainSearchId
        {
            get
            {
                return domainSearchId;
            }
        }
    }
}
