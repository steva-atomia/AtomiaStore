using System.Web.Mvc;
using Atomia.Store.AspNetMvc.Filters;
using Atomia.Store.ExistingCustomer.Adapters;
using Atomia.Store.ExistingCustomer.Models;
using Atomia.Store.Core;

namespace Atomia.Store.ExistingCustomer.Controllers
{

    /// <summary>
    /// Existing customer validation, part of order flow
    /// </summary>
    public sealed class ExistingCustomerController : Controller
    {
        private readonly IContactDataProvider contactDataProvider = DependencyResolver.Current.GetService<IContactDataProvider>();
        private readonly CustomerLoginValidator customerLoginValidator = DependencyResolver.Current.GetService<CustomerLoginValidator>();

        /// <summary>
        /// Existing customer validation handler. Redirects to checkout.
        /// </summary>
        [OrderFlowFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ValidateLogin(CustomerLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var contactData = customerLoginValidator.ValidateCustomerLogin(model.Username, model.Password);

                if (contactData.Valid)
                {
                    var existingCustomer = new ExistingCustomerContactModel();
                    existingCustomer.SetContactData(contactData);

                    contactDataProvider.SaveContactData(existingCustomer);

                    return RedirectToAction("Index", "Checkout");
                }
                else
                {
                    ModelState.AddModelError("customerLogin", "Username or password or both are invalid.");
                }
            }

            return View(new CustomerLoginModel() { Username = model.Username });
        }
    }
}
