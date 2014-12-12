using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core
{
    public abstract class Item
    {
        protected readonly IItemDisplayProvider displayProvider;
        protected readonly ICurrencyProvider currencyProvider;

        public Item(IItemDisplayProvider displayProvider, ICurrencyProvider currencyProvider)
        {
            if (displayProvider == null)
            {
                throw new ArgumentNullException("displayProvider");
            }

            if (currencyProvider == null)
            {
                throw new ArgumentNullException("currencyProvider");
            }

            this.displayProvider = displayProvider;
            this.currencyProvider = currencyProvider;

            CustomAttributes = new List<CustomAttribute>();
        }

        public virtual string ArticleNumber { get; set; }

        public virtual List<CustomAttribute> CustomAttributes { get; set; }

        public virtual string Name
        {
            get
            {
                return this.displayProvider.GetName(this);
            }
        }

        public virtual string Description
        {
            get
            {
                return this.displayProvider.GetDescription(this);
            }
        }
    }
}
