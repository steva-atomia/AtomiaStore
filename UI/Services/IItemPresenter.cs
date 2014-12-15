using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atomia.Store.Core;
using Atomia.Store.AspNetMvc.Models;

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
