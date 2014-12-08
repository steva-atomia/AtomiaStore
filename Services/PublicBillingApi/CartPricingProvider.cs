using Atomia.Store.Core.Cart;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Services.PublicBillingApi
{
    public class CartPricingProvider : ICartPricingProvider
    {
        private readonly IRenewalPeriodProvider renewalPeriodProvider;

        public CartPricingProvider(IRenewalPeriodProvider renewalPeriodProvider)
        {
            this.renewalPeriodProvider = renewalPeriodProvider;
        }

        public Cart CalculatePricing(Cart cart)
        {
            var publicOrder = new PublicOrder
            {
                ResellerId = Guid.Empty,
                Country = "",
                Currency = "",
            };

            var publicOrderItems = new List<PublicOrderItem>();
            var itemNoCounter = 0;

            foreach(var cartItem in cart.CartItems)
            {
                var renewalPeriodId = renewalPeriodProvider.GetRenewalPeriodId(cartItem.ArticleNumber, cartItem.RenewalPeriod.Period, cartItem.RenewalPeriod.Unit);

                publicOrderItems.Add(new PublicOrderItem
                {
                    ItemNumber = cartItem.ArticleNumber,
                    RenewalPeriodId = renewalPeriodId,
                    Quantity = cartItem.Quantity,
                    ItemNo = itemNoCounter
                });

                itemNoCounter += 1;
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

            var publicBillingApi = new AtomiaBillingPublicService();

            var calculatedPublicOrder = publicBillingApi.CalculateOrder(publicOrder);

            cart.SubTotal = calculatedPublicOrder.Subtotal;
            cart.Total = calculatedPublicOrder.Total;
            cart.Tax = calculatedPublicOrder.Taxes.Sum(t => t.Total);

            foreach(var cartItem in cart.CartItems)
            {
                var calculatedItem = calculatedPublicOrder.OrderItems
                    .First(x => x.ItemNumber == cartItem.ArticleNumber && x.RenewalPeriod == cartItem.RenewalPeriod.Period && x.RenewalPeriodUnit == cartItem.RenewalPeriod.Unit);

                cartItem.Price = calculatedItem.Price;
                cartItem.Discount = calculatedItem.Discount;
                cartItem.Quantity = calculatedItem.Quantity;
                cartItem.TaxAmount = calculatedItem.TaxAmount;
            }

            return cart;
        }
    }
}
