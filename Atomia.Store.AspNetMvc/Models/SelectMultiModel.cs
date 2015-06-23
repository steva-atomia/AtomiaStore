using System.Collections.Generic;

namespace Atomia.Store.AspNetMvc.Models
{
    public class SelectMultiModel
    {
        public SelectMultiModel()
        {
            this.Items = new List<SelectItemModel>();
        }

        public string Campaign { get; set; }

        public string Next { get; set; }

        public IEnumerable<SelectItemModel> Items { get; set; }
    }
}
