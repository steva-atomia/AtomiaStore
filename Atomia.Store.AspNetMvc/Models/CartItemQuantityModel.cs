using System;

namespace Atomia.Store.AspNetMvc.Models
{
    public class CartItemQuantityChangeModel
    {
        // [Required]
        public Guid Id { get; set; }

        // [Requried]
        public decimal Quantity { get; set; }
    }
}
