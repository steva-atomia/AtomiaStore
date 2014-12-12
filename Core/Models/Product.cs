using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core
{
    public sealed class Product : Item
    {
        private readonly string articleNumber;
        private readonly decimal price;

        public Product(string articleNumber, decimal price, IItemDisplayProvider itemDisplayProvider, ICurrencyProvider currencyProvider):
            base(itemDisplayProvider, currencyProvider)
        {
            if (string.IsNullOrEmpty(articleNumber))
            {
                throw new ArgumentException("articleNumber");
            }

            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException("price");
            }

            this.articleNumber = articleNumber;
            this.price = price;
        }

        public override string ArticleNumber
        {
            get
            {
                return this.articleNumber;
            }
        }

        public List<RenewalPeriod> RenewalPeriods { get; set; }

        public decimal Price
        {
            get
            {
                return this.price;
            }
        }

        public string DisplayPrice
        {
            get
            {
                return currencyProvider.FormatAmount(Price);
            }
        }
    }
}
