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

        public Item(IItemDisplayProvider displayProvider)
        {
            if (displayProvider == null)
            {
                throw new ArgumentNullException("displayProvider");
            }

            this.displayProvider = displayProvider;
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
