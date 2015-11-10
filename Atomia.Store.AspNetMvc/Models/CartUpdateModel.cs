using System.Collections.Generic;
using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// View model for supplying new cart items and campaign code to update the current <see cref="Cart"/> with.
    /// </summary>
    public class CartUpdateModel
    {
        public CartUpdateModel()
        {
            CartItems = new List<CartItemModel>();
            CustomAttributes = new List<CustomAttribute>();
        }

        /// <summary>
        /// The cart items the cart should contain after update.
        /// </summary>
        public List<CartItemModel> CartItems { get; set; }

        /// <summary>
        /// The custom attributes the cart should contain after update.
        /// </summary>
        public List<CustomAttribute> CustomAttributes { get; set; }

        /// <summary>
        /// The campaign code the cart should contain after update.
        /// </summary>
        public string CampaignCode { get; set; }
    }
}
