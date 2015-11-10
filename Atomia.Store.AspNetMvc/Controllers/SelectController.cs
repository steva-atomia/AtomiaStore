using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System.Web.Mvc;
using System;

namespace Atomia.Store.AspNetMvc.Controllers
{
    public class SelectController : Controller
    {
        private readonly ICartProvider cartProvider = DependencyResolver.Current.GetService<ICartProvider>();

        /// <summary>
        /// Add a single item with optional campaign code and custom attributes to cart.
        /// </summary>
        [HttpPost]
        public ActionResult Index(SelectSingleModel model)
        {
            if (ModelState.IsValid)
            {
                var cart = cartProvider.GetCart();

                try
                {
                    cart.AddItem(model.CartItem);
                }
                catch(Exception)
                {
                    // Broadly catch and ignore exceptions to avoid interuption to user from faulty external forms.
                }

                if (model.CartCustomAttributes != null)
                {
                    foreach (var attr in model.CartCustomAttributes)
                    {
                        cart.SetCustomAttribute(attr.Name, attr.Value);
                    }
                }

                if (!string.IsNullOrEmpty(model.Campaign))
                {
                    cart.SetCampaignCode(model.Campaign);
                }

                if (!string.IsNullOrEmpty(model.Next))
                {
                    return Redirect(model.Next);
                }
            }

            return RedirectToRoute("OrderFlowStart");
        }

        /// <summary>
        /// Add multiple items with optional campaign code and custom attributes to cart.
        /// </summary>
        [HttpPost]
        public ActionResult Multi(SelectMultiModel model)
        {
            if(ModelState.IsValid)
            {
                var cart = cartProvider.GetCart();

                if (model.Items != null)
                {
                    foreach (var item in model.Items)
                    {
                        try
                        {
                            cart.AddItem(item.CartItem);
                        }
                        catch(Exception)
                        {
                            // Broadly catch and ignore exceptions to avoid interuption to user from faulty external forms.
                        }
                    }
                }

                if (model.CartCustomAttributes != null)
                {
                    foreach (var attr in model.CartCustomAttributes)
                    {
                        cart.SetCustomAttribute(attr.Name, attr.Value);
                    }
                }

                if (!string.IsNullOrEmpty(model.Campaign))
                {
                    cart.SetCampaignCode(model.Campaign);
                }

                if (!string.IsNullOrEmpty(model.Next))
                {
                    return Redirect(model.Next);
                }
            }

            return RedirectToRoute("OrderFlowStart");
        }

        /// <summary>
        /// Add a campaign code to cart.
        /// </summary>
        public ActionResult Campaign(string campaign, string next)
        {
            if (!string.IsNullOrEmpty(campaign))
            {
                var cart = cartProvider.GetCart();

                cart.SetCampaignCode(campaign);
            }

            if (!string.IsNullOrEmpty(next))
            {
                return Redirect(next);
            }

            return RedirectToRoute("OrderFlowStart");
        }

        /// <summary>
        /// Add custom attributes to cart.
        /// </summary>
        public ActionResult Attrs(SelectAttrsModel model)
        {
            if (ModelState.IsValid)
            {
                var cart = cartProvider.GetCart();

                try
                {
                    if (model.CartCustomAttributes != null)
                    {
                        foreach (var attr in model.CartCustomAttributes)
                        {
                            cart.SetCustomAttribute(attr.Name, attr.Value);
                        }
                    }
                }
                catch (Exception)
                {
                    // Broadly catch and ignore exceptions to avoid interuption to user from faulty external forms.
                }

                if (!string.IsNullOrEmpty(model.Campaign))
                {
                    cart.SetCampaignCode(model.Campaign);
                }

                if (!string.IsNullOrEmpty(model.Next))
                {
                    return Redirect(model.Next);
                }
            }

            return RedirectToRoute("OrderFlowStart");
        }
    }
}
