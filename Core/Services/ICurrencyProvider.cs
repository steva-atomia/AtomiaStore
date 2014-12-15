
namespace Atomia.Store.Core
{
    public interface ICurrencyProvider
    {
        string GetCurrencyCode();

        void SetCurrencyCode(string currencyCode);
    }
}
