using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core
{
    public interface IVatDataProvider
    {
        string VatNumber { get; set; }

        string CountryCode { get; }
    }
}
