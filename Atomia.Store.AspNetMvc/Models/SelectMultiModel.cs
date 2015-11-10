using System.Collections.Generic;
using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Models
{
    public class SelectMultiModel
    {
        public SelectMultiModel()
        {
            this.Items = new List<SelectItemModel>();
            this.CartCustomAttributes = new List<CustomAttribute>();
        }

        public string Campaign { get; set; }

        public string Next { get; set; }

        public IEnumerable<SelectItemModel> Items { get; set; }

        public IEnumerable<CustomAttribute> CartCustomAttributes { get; set; }
    }
}
