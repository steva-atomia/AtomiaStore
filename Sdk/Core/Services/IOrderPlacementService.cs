using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Atomia.OrderPage.Sdk.Core.Models;

namespace Atomia.OrderPage.Sdk.Core.Services
{
    public interface IOrderPlacementService
    {
        RedirectResult PlaceOrder(CheckoutModel model);
    }
}