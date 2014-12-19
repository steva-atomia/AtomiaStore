using System;

namespace Atomia.Store.PublicBillingApi
{
    public interface IResellerProvider
    {
        Guid GetResellerId();
    }

    // TODO: Implement IResellerProvider
}
