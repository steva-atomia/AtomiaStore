using Atomia.Store.AspNetMvc.Helpers;
using Atomia.Store.Core;

namespace Atomia.Store.Fakes.Adapters
{
    public sealed class FakeRootResellerIdentifierProvider : IResellerIdentifierProvider
    {
        public ResellerIdentifier GetResellerIdentifier()
        {
            return new ResellerIdentifier
            {
                AccountHash = "184ec886dc90d2c1b5e50b6afe80db01",
                BaseUrl = BaseUriHelper.GetBaseUriString()
            };
        }


        public void SetResellerIdentifier(ResellerIdentifier identifier)
        {
            
        }
    }
}
