
namespace Atomia.Store.Core
{
    public interface ICurrencyPreferenceProvider
    {
        void SetPreferredCurrency(Currency currency);

        Currency GetCurrentCurrency();
    }
}
