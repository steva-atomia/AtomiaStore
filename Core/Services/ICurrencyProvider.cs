using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core
{
    public interface ICurrencyProvider
    {
        string GetCurrencyCode();

        void SetCurrencyCode(string currencyCode);

        string FormatAmount(decimal amount);
    }
}
