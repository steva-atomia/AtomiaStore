using System;

namespace Atomia.Store.AspNetMvc.Models
{
    public class CartItemRemoveAttributeModel
    {
        // [Required]
        public Guid Id { get; set; }

        // [Requried]
        public string AttributeName { get; set; }
    }
}
