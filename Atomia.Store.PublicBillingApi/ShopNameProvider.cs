using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Atomia.Store.PublicBillingApi
{
    public interface IShopNameProvider
    {
        string GetShopName();
    }


    public class DefaultShopNameProvider : IShopNameProvider
    {
        public string GetShopName()
        {
            return "";
        }
    }

    public class DomainShopNameProvider : IShopNameProvider
    {
        public string GetShopName()
        {
            var currentUrl = HttpContext.Current.Request.Url;
            var domainName = currentUrl.Authority;

            return domainName;
        }
    }
}
