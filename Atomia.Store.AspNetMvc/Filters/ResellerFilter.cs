using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Atomia.Store.Core;
using Atomia.Store.AspNetMvc.Helpers;

namespace Atomia.Store.AspNetMvc.Filters
{
    public class ResellerFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            if (filterContext.Controller.ControllerContext.IsChildAction || request.IsAjaxRequest())
            {
                return;
            }

            if (request.QueryString["reseller"] != null)
            {   
                var identifier = new ResellerIdentifier()
                {
                    AccountHash = request.QueryString["reseller"],
                    BaseUrl = BaseUriHelper.GetBaseUriString()
                };

                var resellerIdentifierProvider = DependencyResolver.Current.GetService<IResellerIdentifierProvider>();
                resellerIdentifierProvider.SetResellerIdentifier(identifier);
            }
        }
    }
}
