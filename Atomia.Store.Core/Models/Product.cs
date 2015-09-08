using System.Collections.Generic;

namespace Atomia.Store.Core
{
    /// <summary>
    /// A product in the store.
    /// </summary>
    public sealed class Product
    {
        /// <summary>
        /// Unique article number for the product
        /// </summary>
        public string ArticleNumber { get; set; }

        /// <summary>
        /// Human readable name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Human readable description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// All categories the product belongs to.
        /// </summary>
        public List<Category> Categories { get; set; }

        /// <summary>
        /// Custom data needed for the product
        /// </summary>
        public List<CustomAttribute> CustomAttributes { get; set; }

        /// <summary>
        /// All combinations of <see cref="RenewalPeriod"/> and price available for the product
        /// </summary>
        public List<PricingVariant> PricingVariants { get; set; }
    }
}
