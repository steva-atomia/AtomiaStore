using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atomia.OrderPage.Sdk.Infrastructure
{
    public interface IModelProvider
    {
        TViewModel Create<TViewModel>();
    }
}
