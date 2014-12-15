using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.AspNetMvc.Models
{
    public class CartItemQuantityModel
    {
        // [Required]
        public int Id { get; set; }

        // [Requried]
        public decimal Quantity { get; set; }
    }
}
