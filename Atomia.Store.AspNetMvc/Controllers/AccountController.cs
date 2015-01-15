using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.AspNetMvc.Providers;
using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public sealed class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            // var model = DependencyResolver.Current.GetService<AccountViewModel>();

            return View();
        }
    }
}
