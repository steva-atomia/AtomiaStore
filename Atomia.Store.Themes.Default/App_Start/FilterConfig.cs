using Atomia.Store.AspNetMvc.Filters;
using System.Web.Mvc;

namespace Atomia.Store.Themes.Default
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ResellerFilter(), 1);
            filters.Add(new LanguageFilter(), 2);
            filters.Add(new ThemeFilter(), 3);
            filters.Add(new CurrencyFilter(), 4);
        }
    }
}