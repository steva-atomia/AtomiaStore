using System;
using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public sealed class Product : Item
    {
        private readonly string articleNumber;

        public Product(string articleNumber, IItemDisplayProvider itemDisplayProvider, ICurrencyProvider currencyProvider):
            base(itemDisplayProvider, currencyProvider)
        {
            if (string.IsNullOrEmpty(articleNumber))
            {
                throw new ArgumentException("articleNumber");
            }

            this.articleNumber = articleNumber;
        }

        public override string ArticleNumber
        {
            get
            {
                return this.articleNumber;
            }
        }

        public List<PricedRenewalPeriod> RenewalPeriodChoices { get; set; }
    }
}
