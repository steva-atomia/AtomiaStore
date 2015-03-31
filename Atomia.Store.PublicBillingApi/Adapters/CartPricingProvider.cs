using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    /// <summary>
    /// <see cref="Atomia.Store.Core.ICartPricingService"/> to calculate cart based on current reseller's prices in Atomia Billing.
    /// </summary>
    public sealed class CartPricingProvider : PublicBillingApiClient, ICartPricingService
    {
        private readonly IResellerProvider resellerProvider;
        private readonly ICurrencyPreferenceProvider currencyPreferenceProvider;
        private readonly ICountryProvider countryProvider;
        private readonly RenewalPeriodProvider renewalPeriodProvider;

        /// <summary>
        /// Construct a new instance of CartPricingProvider
        /// </summary>
        public CartPricingProvider(IResellerProvider resellerProvider, ICurrencyPreferenceProvider currencyPreferenceProvider, ICountryProvider countryProvider, RenewalPeriodProvider renewalPeriodProvider, PublicBillingApiProxy billingApi)
            : base(billingApi)
        {
            if (resellerProvider == null)
            {
                throw new ArgumentNullException("resellerProvider");
            }

            if (currencyPreferenceProvider == null)
            {
                throw new ArgumentNullException("currencyPreferenceProvider");
            }

            if (countryProvider == null)
            {
                throw new ArgumentNullException("countryProvider");
            }

            if (renewalPeriodProvider == null)
            {
                throw new ArgumentNullException("renewalPeriodProvider");
            }

            this.resellerProvider = resellerProvider;
            this.currencyPreferenceProvider = currencyPreferenceProvider;
            this.countryProvider = countryProvider;
            this.renewalPeriodProvider = renewalPeriodProvider;
        }

        /// <inheritdoc />
        public Cart CalculatePricing(Cart cart)
        {
            var publicOrder = CreateBasicOrder();

            var publicOrderItems = new List<PublicOrderItem>();
            var itemNo = 0;

            foreach(var cartItem in cart.CartItems)
            {
                var renewalPeriodId = renewalPeriodProvider.GetRenewalPeriodId(cartItem);

                publicOrderItems.Add(new PublicOrderItem
                {
                    ItemNumber = cartItem.ArticleNumber,
                    RenewalPeriodId = renewalPeriodId,
                    Quantity = cartItem.Quantity,
                    ItemNo = itemNo++
                });
            }
            publicOrder.OrderItems = publicOrderItems.ToArray();

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

            var calculatedPublicOrder = BillingApi.CalculateOrder(publicOrder);

            cart.SetPricing(calculatedPublicOrder.Subtotal, calculatedPublicOrder.Taxes.Sum(t => t.Total), calculatedPublicOrder.Total);

            foreach(var cartItem in cart.CartItems)
            {
                PublicOrderItem calculatedItem;
                var calcOrderItems = calculatedPublicOrder.OrderItems;

                if (cartItem.RenewalPeriod == null)
                {
                    calculatedItem = calcOrderItems.First(x => 
                        x.ItemNumber == cartItem.ArticleNumber 
                        && x.RenewalPeriod == 0);
                }
                else
                {
                   calculatedItem = calcOrderItems.First(x => 
                       x.ItemNumber == cartItem.ArticleNumber 
                       && x.RenewalPeriod == cartItem.RenewalPeriod.Period 
                       && x.RenewalPeriodUnit.ToUpper() == cartItem.RenewalPeriod.Unit);
                }

                cartItem.SetPricing(calculatedItem.Price, calculatedItem.Discount, calculatedItem.TaxAmount);
                cartItem.Quantity = calculatedItem.Quantity;
            }

            return cart;
        }

        private PublicOrder CreateBasicOrder()
        {
            return new PublicOrder
            {
                ResellerId = resellerProvider.GetReseller().Id,
                Country = countryProvider.GetDefaultCountry().Code,
                Currency = currencyPreferenceProvider.GetCurrentCurrency().Code
            };
        }
    }
}
