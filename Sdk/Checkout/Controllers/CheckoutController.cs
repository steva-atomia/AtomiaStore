using System.Web.Mvc;
using Atomia.OrderPage.Sdk.Checkout.Models;
using Atomia.OrderPage.Sdk.Checkout.Services;
using Atomia.OrderPage.Sdk.Common.Models;

namespace Atomia.OrderPage.Sdk.Checkout.Controllers
{
    public class CheckoutController : Controller
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
                var redirectResult = orderPlacementService.PlaceOrder(model);
                
                return redirectResult;
            }

            return View(model);
        }
    }
}
