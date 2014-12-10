using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atomia.Store.Services.PublicBillingApi
{
    public interface IResellerProvider
    {
        Guid GetCurrentResellerId();
    }
}
