﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core
{
    public interface ICartRepository
    {
        Cart GetCart();

        void SaveCart(Cart cart);
    }
}
