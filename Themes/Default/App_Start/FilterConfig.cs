using Atomia.Web.Base.ActionFilters;
using System.Web.Mvc;

namespace Atomia.Store.Themes.Default
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new InternationalizationAttribute());
        }
    }
}