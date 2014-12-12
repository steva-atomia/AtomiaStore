
namespace Atomia.Store.Core
{
    public interface IItemDisplayProvider
    {
        string GetName(Item item);

        string GetDescription(Item item);
    }
}
