using System.Collections.Generic;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// Default model for the product listing JSON API (ProductListingController::GetData)
    /// </summary>
    public class ProductListingModel
    {
        /// <summary>
        /// Products to list
        /// </summary>
        public virtual ICollection<ProductModel> Products { get; set; }

        /// <summary>
        /// Query that was used to get the product list
        /// </summary>
        public virtual string Query { get; set; }
    }
}