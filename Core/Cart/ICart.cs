using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core.Products;

namespace Atomia.Store.Core.Cart
{
    public interface ICart
    {
        IEnumerable<CartItem> CartItems { get; }

        IEnumerable<string> CampaignCodes { get; }

        Pricing Pricing { get; }

        void Add(CartItem item);

        void Remove(CartItem item);
        
        void Add(string campaignCode);
        
        void Remove(string campaignCode);
        
        void Clear();
    }
}
