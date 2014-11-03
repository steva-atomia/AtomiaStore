using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atomia.OrderPage.Core.Models;
using Atomia.OrderPage.Core.Services;
using Atomia.OrderPage.Core.Infrastructure;

namespace Atomia.OrderPage.Core.Controllers
{
    public sealed class DomainsController : Controller
    {
        private readonly IModelProvider modelProvider;

        public DomainsController(IModelProvider modelProvider)
        {
            this.modelProvider = modelProvider;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = modelProvider.Create<DomainsModel>();

            return View(model);
        }
    }
}
