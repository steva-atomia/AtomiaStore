using System.Web;
using System.Web.Mvc;
using Atomia.Web.Base.ActionFilters;
using Atomia.OrderPage.ActionTrail;

namespace Atomia.OrderPage.Themes.Default
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogErrorAttribute());
            filters.Add(new InternationalizationAttribute());
        }
    }
}