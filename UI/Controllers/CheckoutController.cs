using System.Web.Mvc;
using Atomia.Store.Core;
using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.ViewModels;

namespace Atomia.Store.AspNetMvc.Controllers
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
            var model = modelProvider.Create<CheckoutViewModel>();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CheckoutViewModel model)
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
            var model = modelProvider.Create<CheckoutViewModel>();

            return PartialView(model);
        }
    }
}
