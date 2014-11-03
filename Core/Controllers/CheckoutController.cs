using System.Web.Mvc;
using Atomia.OrderPage.Core.Models;
using Atomia.OrderPage.Core.Services;
using Atomia.OrderPage.Core.Infrastructure;

namespace Atomia.OrderPage.Core.Controllers
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

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CheckoutModel model)
        {
            if (ModelState.IsValid)
            {
                //var order = formHandler.Handle(model);
                //var redirectResult = orderPlacementService.PlaceOrder(order);
                
                //return redirectResult;
            }

            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult Partial()
        {
            var model = modelProvider.Create<CheckoutModel>();

            return PartialView(model);
        }
    }
}
