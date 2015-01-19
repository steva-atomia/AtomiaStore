using System;

namespace Atomia.Store.AspNetMvc.Models
{
    public class CartItemSetAttributeModel
    {
        // [Required]
        public Guid Id { get; set; }

        // [Requried]
        public string AttributeName { get; set; }

        // [Requried]
        public string AttributeValue { get; set; }
    }
}
