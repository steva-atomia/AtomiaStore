using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System.Collections.Generic;
using System.Linq;
using System;
using Atomia.Web.Plugin.ProductsProvider;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    public class CartPricingProvider : PublicBillingApiClient, ICartPricingService
    {
        private readonly IProductsProvider productsProvider;
        private readonly IResellerProvider resellerProvider;
        private readonly ICurrencyPreferenceProvider currencyPreferenceProvider;
        private readonly ICountryProvider countryProvider;

        public CartPricingProvider(IProductsProvider productsProvider, IResellerProvider resellerProvider, ICurrencyPreferenceProvider currencyPreferenceProvider, ICountryProvider countryProvider, PublicBillingApiProxy billingApi)
            : base(billingApi)
        {
            if (productsProvider == null)
            {
                throw new ArgumentNullException("productsProvider");
            }

            if (resellerProvider == null)
            {
                throw new ArgumentNullException("productsProvider");
            }

            if (currencyPreferenceProvider == null)
            {
                throw new ArgumentNullException("currencyPreferenceProvider");
            }

            if (countryProvider == null)
            {
                throw new ArgumentNullException("countryProvider");
            }

            this.productsProvider = productsProvider;
            this.resellerProvider = resellerProvider;
            this.currencyPreferenceProvider = currencyPreferenceProvider;
            this.countryProvider = countryProvider;
        }

        public Cart CalculatePricing(Cart cart)
        {
            var publicOrder = CreateBasicOrder();

            var publicOrderItems = new List<PublicOrderItem>();
            var itemNo = 0;

            foreach(var cartItem in cart.CartItems)
            {
                var renewalPeriodId = GetRenewalPeriodId(cartItem);

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
                var calculatedItem = calculatedPublicOrder.OrderItems
                    .First(x => x.ItemNumber == cartItem.ArticleNumber 
                        && x.RenewalPeriod == cartItem.RenewalPeriod.Period 
                        && x.RenewalPeriodUnit.ToUpper() == cartItem.RenewalPeriod.Unit);

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

        private Guid GetRenewalPeriodId(CartItem cartItem)
        {
            if (cartItem.RenewalPeriod == null)
            {
                return Guid.Empty;
            }

            var period = cartItem.RenewalPeriod.Period;
            var unit = cartItem.RenewalPeriod.Unit;
            var product = productsProvider.GetShopProductsByArticleNumbers(resellerProvider.GetReseller().Id, "", new List<string> { cartItem.ArticleNumber }).FirstOrDefault();

            if (product == null)
            {
                throw new InvalidOperationException(String.Format("No product with articlenumber {0} found", cartItem.ArticleNumber));
            }

            var apiRenewalPeriod = product.RenewalPeriods
                .FirstOrDefault(r => r.RenewalPeriodUnit.ToUpper() == unit && r.RenewalPeriodValue == period);

            if (apiRenewalPeriod == null)
            {
                throw new InvalidOperationException(String.Format("No renewal period {0} {1} found", period, unit));
            }

            return apiRenewalPeriod.Id;
        }
    }
}
