using Atomia.Store.Core;
using System.Collections.Generic;

namespace Atomia.Store.AspNetMvc.Services
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
    }
}
