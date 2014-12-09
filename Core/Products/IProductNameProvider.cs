using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atomia.Store.Core.Products
{
    public interface IProductNameProvider
    {
        string GetProductName(string articleNumber);
    }
}
