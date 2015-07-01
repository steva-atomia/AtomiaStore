using Atomia.Store.Core;
using System;
using System.Configuration;
using System.Web;

namespace Atomia.Store.Themes.Default.Adapters
{
    public class DefaultCampaignPricingProvider : ICartPricingService
    {
        private readonly ICartPricingService baseCartPricingService;

        public DefaultCampaignPricingProvider(ICartPricingService baseCartPricingService)
        {
            if (baseCartPricingService == null)
            {
                throw new ArgumentNullException("baseCartPricingService");
            }

            this.baseCartPricingService = baseCartPricingService;
        }

        public Cart CalculatePricing(Cart cart)
        {
            /* Using session to mark if default campaign code has already been added is done so if customer
             * (for some reason) has removed the campaign code from the cart it will not show up again. 
             * 
             * A minor unhandled edge-case is that if a customer places two or more orders before the session has timed out
             * the default campaign code will not be applied to the later orders since the marker is stored in the session 
             * and not the cart, so it will not be cleared together with the cart on order completion.
             */ 
            
            var alreadyAdded = HttpContext.Current.Session["DefaultCampaignPricingProvider_Added"] != null 
                 ? true
                 : false;

            if (string.IsNullOrEmpty(cart.CampaignCode) && !alreadyAdded)
            {
                var defaultCampaignCode = ConfigurationManager.AppSettings["DefaultCampaignCode"] as String;

                if (!string.IsNullOrEmpty(defaultCampaignCode))
                {
                    cart.SetCampaignCode(defaultCampaignCode);
                    HttpContext.Current.Session["DefaultCampaignPricingProvider_Added"] = true;
                }
            }
            
            var calculatedCart = baseCartPricingService.CalculatePricing(cart);

            return calculatedCart;
        }
    }
}
