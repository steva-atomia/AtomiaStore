using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atomia.OrderPage.UI.Infrastructure
{
    public interface IInputHandler<TInput, TDataObject>
    {
        TDataObject Translate(TInput input, TDataObject dataObject);
    }
}
