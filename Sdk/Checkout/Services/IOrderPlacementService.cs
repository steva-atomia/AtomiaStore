using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atomia.OrderPage.Sdk.Checkout.Models;

namespace Atomia.OrderPage.Sdk.Checkout.Services
{
    public interface IOrderPlacementService
    {
        RedirectResult PlaceOrder(CheckoutModel model);
    }
}