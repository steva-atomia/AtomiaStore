using Atomia.Store.Core;
using System.Collections.Generic;

namespace Atomia.Store.AspNetMvc.Models
{
    public abstract class ProductsViewModel
    {
        public IList<Product> Products { get; set; }
        public string Category { get; set; }
    }
}