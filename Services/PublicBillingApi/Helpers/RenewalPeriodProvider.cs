using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atomia.Store.Services.PublicBillingApi
{
    public interface IRenewalPeriodProvider
    {
        Guid GetRenewalPeriodId(string articleNumber, int renewalPeriod, string renewalPeriodUnit);
    }

    // TODO: Implement RenewalPeriodProvider
}
