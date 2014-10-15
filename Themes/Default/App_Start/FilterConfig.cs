using System.Web;
using System.Web.Mvc;

namespace Atomia.OrderPage.Themes.Default
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}