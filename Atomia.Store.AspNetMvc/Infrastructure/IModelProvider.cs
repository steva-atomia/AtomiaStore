
namespace Atomia.Store.AspNetMvc.Infrastructure
{
    public interface IModelProvider
    {
        TViewModel Create<TViewModel>();
    }
}
