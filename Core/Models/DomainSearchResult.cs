using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atomia.OrderPage.Core.Models
{
    public class DomainSearchResult
    {
        public string DomainName { get; set; }

        public decimal Price { get; set; }

        public string CurrencyCode { get; set; }

        public string Status { get; set; }
    }
}
