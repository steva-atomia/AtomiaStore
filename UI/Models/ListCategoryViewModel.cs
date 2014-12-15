﻿using Atomia.Store.Core;
using System.Collections.Generic;

namespace Atomia.Store.AspNetMvc.Models
{
    public abstract class ListCategoryViewModel
    {
        public virtual IList<ProductModel> Products { get; set; }

        public string Category { get; set; }
    }
}