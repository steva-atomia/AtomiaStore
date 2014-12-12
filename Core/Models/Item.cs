using System;
using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public abstract class Item
    {
        protected readonly IItemDisplayProvider itemDisplayProvider;
        protected readonly ICurrencyProvider currencyProvider;

        public Item(IItemDisplayProvider itemDisplayProvider, ICurrencyProvider currencyProvider)
        {
            if (itemDisplayProvider == null)
            {
                throw new ArgumentNullException("displayProvider");
            }

            if (currencyProvider == null)
            {
                throw new ArgumentNullException("currencyProvider");
            }

            this.itemDisplayProvider = itemDisplayProvider;
            this.currencyProvider = currencyProvider;

            CustomAttributes = new List<CustomAttribute>();
        }

        public virtual string ArticleNumber { get; set; }

        public virtual List<CustomAttribute> CustomAttributes { get; set; }

        public virtual string Name
        {
            get
            {
                return this.itemDisplayProvider.GetName(this);
            }
        }

        public virtual string Description
        {
            get
            {
                return this.itemDisplayProvider.GetDescription(this);
            }
        }
    }
}
