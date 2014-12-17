
namespace Atomia.Store.AspNetMvc.Services
{
    public interface ICurrencyFormatter
    {
        string FormatAmount(decimal amount);
    }
}
