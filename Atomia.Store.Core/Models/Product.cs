using System;
using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public sealed class Product
    {
        public string ArticleNumber { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public List<CustomAttribute> CustomAttributes { get; set; }

        public List<PricingVariant> PricingVariants { get; set; }
    }
}
