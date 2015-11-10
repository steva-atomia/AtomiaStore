using Atomia.Store.Core;
using System.Collections.Generic;

namespace Atomia.Store.AspNetMvc.Models
{
    public class SelectAttrsModel
    {
        public SelectAttrsModel()
        {
            this.CartCustomAttributes = new List<CustomAttribute>();
        }

        public string Next { get; set; }

        public string Campaign { get; set; }

        public IEnumerable<CustomAttribute> CartCustomAttributes { get; set; }
    }
}
