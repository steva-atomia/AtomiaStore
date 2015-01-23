using System.Collections.Generic;

namespace Atomia.Store.AspNetMvc.Models
{
    public class ProductListingDataModel
    {
        public virtual ICollection<ProductModel> Products { get; set; }

        public virtual string Query { get; set; }
    }
}