using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core
{
    public abstract class Product
    {
        public string ArticleNumber { get; set; }

        public List<RenewalPeriod> RenewalPeriods { get; set; }

        public decimal Price { get; set; }

        public string CurrencyCode { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }
    }
}
