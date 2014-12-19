using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    public class CartPricingProvider : ICartPricingService
    {
        private readonly IOrderCreator orderCreator;
        private readonly IOrderCalculator orderCalculator;
        private readonly IRenewalPeriodProvider renewalPeriodProvider;

        public CartPricingProvider(IOrderCreator orderCreator, IOrderCalculator orderCalculator, IRenewalPeriodProvider renewalPeriodProvider)
        {
            this.orderCreator = orderCreator;
            this.orderCalculator = orderCalculator;
            this.renewalPeriodProvider = renewalPeriodProvider;   
        }

        public Cart CalculatePricing(Cart cart)
        {
            var publicOrder = orderCreator.CreateBasicOrder();

            var publicOrderItems = new List<PublicOrderItem>();
            var itemNo = 0;

            foreach(var cartItem in cart.CartItems)
            {
                var renewalPeriodId = renewalPeriodProvider.GetRenewalPeriodId(cartItem.ArticleNumber, cartItem.RenewalPeriod.Period, cartItem.RenewalPeriod.Unit);

                publicOrderItems.Add(new PublicOrderItem
                {
                    ItemNumber = cartItem.ArticleNumber,
                    RenewalPeriodId = renewalPeriodId,
                    Quantity = cartItem.Quantity,
                    ItemNo = itemNo++
                });
            }

            var publicOrderCustomData = new List<PublicOrderCustomData>();
            if (!string.IsNullOrEmpty(cart.CampaignCode))
            {
                publicOrderCustomData.Add(new PublicOrderCustomData
                    {
                        Name = "CampaignCode",
                        Value = cart.CampaignCode
                    }
                );
            }
            publicOrder.CustomData = publicOrderCustomData.ToArray();

            var calculatedPublicOrder = orderCalculator.CalculateOrder(publicOrder);

            cart.SetPricing(calculatedPublicOrder.Subtotal, calculatedPublicOrder.Taxes.Sum(t => t.Total), calculatedPublicOrder.Total);

            foreach(var cartItem in cart.CartItems)
            {
                var calculatedItem = calculatedPublicOrder.OrderItems
                    .First(x => x.ItemNumber == cartItem.ArticleNumber && x.RenewalPeriod == cartItem.RenewalPeriod.Period && x.RenewalPeriodUnit == cartItem.RenewalPeriod.Unit);

                cartItem.SetPricing(calculatedItem.Price, calculatedItem.Discount, calculatedItem.TaxAmount);
                cartItem.Quantity = calculatedItem.Quantity;
            }

            return cart;
        }
    }
}
