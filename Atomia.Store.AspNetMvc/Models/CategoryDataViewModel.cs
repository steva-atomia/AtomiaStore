using System.Collections.Generic;

namespace Atomia.Store.AspNetMvc.Models
{
    public abstract class CategoryDataViewModel
    {
        public virtual ICollection<ProductModel> Products { get; set; }

        public virtual string Category { get; set; }
    }
}