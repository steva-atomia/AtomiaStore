using System.Collections.Generic;

namespace Atomia.Store.AspNetMvc.Models
{
    public class CartUpdateModel
    {
        public CartUpdateModel()
        {
            CartItems = new List<CartItemModel>();
        }

        public List<CartItemModel> CartItems { get; set; }

        public string CampaignCode { get; set; }
    }
}
