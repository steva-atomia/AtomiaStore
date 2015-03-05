using Atomia.Store.AspNetMvc.Helpers;
using Atomia.Store.Core;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Filters
{
    public sealed class ResellerFilter : ActionFilterAttribute
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
