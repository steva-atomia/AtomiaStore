using Atomia.Store.Core;
using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public interface IPresentableItem
    {
        string ArticleNumber { get; }

        List<CustomAttribute> CustomAttributes { get; }
    }

    public interface IItemPresenter
    {
        string GetName(IPresentableItem item);

        string GetDescription(IPresentableItem item);

        string GetCategory(IPresentableItem item);
    }
}
