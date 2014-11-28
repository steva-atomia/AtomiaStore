using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core.Cart
{
    public interface ICartService
    {
        ICart GetCart();

        void SaveCart(ICart cart);
    }
}
