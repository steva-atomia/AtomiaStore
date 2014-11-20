using System.Web;
using System.Web.Mvc;
using Atomia.Web.Base.ActionFilters;


namespace Atomia.OrderPage.Themes.Default
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new InternationalizationAttribute());
        }
    }
}