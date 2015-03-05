
namespace Atomia.Store.Core
{
    public interface IResellerIdentifierProvider
    {
        ResellerIdentifier GetResellerIdentifier();

        void SetResellerIdentifier(ResellerIdentifier identifier);
    }
}
