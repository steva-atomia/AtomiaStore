
namespace Atomia.Store.Core
{
    public interface ICurrencyProvider
    {
        string GetCurrencyCode();

        void SetCurrencyCode(string currencyCode);

        string FormatAmount(decimal amount);
    }
}
