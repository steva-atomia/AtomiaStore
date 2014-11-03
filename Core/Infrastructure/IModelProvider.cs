using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atomia.OrderPage.Core.Infrastructure
{
    public interface IModelProvider
    {
        TViewModel Create<TViewModel>();
    }
}
