using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.ViewModels
{
    public abstract class ProductsViewModel
    {
        public List<Product> Products { get; set; }
    }
}