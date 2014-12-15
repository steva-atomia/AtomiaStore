using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atomia.Store.AspNetMvc.Services
{
    public interface ICurrencyFormatter
    {
        string FormatAmount(decimal amount);
    }
}
