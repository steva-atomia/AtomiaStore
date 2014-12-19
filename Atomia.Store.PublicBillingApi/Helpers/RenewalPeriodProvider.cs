using System;

namespace Atomia.Store.PublicBillingApi
{
    public interface IRenewalPeriodProvider
    {
        Guid GetRenewalPeriodId(string articleNumber, int renewalPeriod, string renewalPeriodUnit);
    }

    // TODO: Implement RenewalPeriodProvider
}
