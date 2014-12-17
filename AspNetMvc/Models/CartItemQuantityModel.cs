
namespace Atomia.Store.AspNetMvc.Models
{
    public class CartItemQuantityChangeModel
    {
        // [Required]
        public int Id { get; set; }

        // [Requried]
        public decimal Quantity { get; set; }
    }
}
