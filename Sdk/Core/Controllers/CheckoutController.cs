using System.Web.Mvc;
using Atomia.OrderPage.Sdk.Core.Models;
using Atomia.OrderPage.Sdk.Core.Services;
using Atomia.OrderPage.Sdk.Infrastructure;

namespace Atomia.OrderPage.Sdk.Core.Controllers
{
    public sealed class CheckoutController : Controller
    {
        private readonly IModelProvider modelProvider;
        private readonly IOrderPlacementService orderPlacementService;

        public CheckoutController(IModelProvider modelProvider, IOrderPlacementService orderPlacementService)
        {
            this.modelProvider = modelProvider;
            this.orderPlacementService = orderPlacementService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = modelProvider.Create<CheckoutModel>();

            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(CheckoutModel model)
        {
            if (ModelState.IsValid)
            {
                var redirectResult = orderPlacementService.PlaceOrder(model);
                
                return redirectResult;
            }

            return View("Index", model);
        }

        [ChildActionOnly]
        public PartialViewResult Partial()
        {
            var model = modelProvider.Create<CheckoutModel>();

            return PartialView("_CheckoutPartial", model);
        }
    }
}
