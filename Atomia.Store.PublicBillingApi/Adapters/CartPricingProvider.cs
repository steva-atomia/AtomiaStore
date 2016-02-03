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
        private readonly bool pricesIncludeVat;
        private readonly bool inclusiveTaxCalculationType;

        /// <summary>
        /// Construct a new instance of CartPricingProvider
        /// </summary>
        public CartPricingProvider(
            IResellerProvider resellerProvider, 
            ICurrencyPreferenceProvider currencyPreferenceProvider, 
            ICountryProvider countryProvider, 
            RenewalPeriodProvider renewalPeriodProvider, 
            IVatDisplayPreferenceProvider vatDisplayPreferenceProvider, 
            PublicBillingApiProxy billingApi)
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

            if (vatDisplayPreferenceProvider == null)
            {
                throw new ArgumentNullException("vatDisplayPreferenceProvider");
            }

            this.resellerProvider = resellerProvider;
            this.currencyPreferenceProvider = currencyPreferenceProvider;
            this.countryProvider = countryProvider;
            this.renewalPeriodProvider = renewalPeriodProvider;
            this.pricesIncludeVat = vatDisplayPreferenceProvider.ShowPricesIncludingVat();
            this.inclusiveTaxCalculationType = resellerProvider.GetReseller().InclusiveTaxCalculationType;
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
            if (cart.CustomAttributes != null)
            {
                foreach (Core.CustomAttribute attr in cart.CustomAttributes)
                {
                    publicOrderCustomData.Add(new PublicOrderCustomData { Name = attr.Name, Value = attr.Value });
                }
            }

            publicOrder.CustomData = publicOrderCustomData.ToArray();

            var calculatedPublicOrder = BillingApi.CalculateOrder(publicOrder);

            IEnumerable<Tax> taxes = new List<Tax>();

            if (calculatedPublicOrder.Taxes != null && calculatedPublicOrder.Taxes.Count() > 0)
            {
                taxes = calculatedPublicOrder.Taxes.Select(t => new Tax(t.Name, t.Total, t.Percent));
            }

            var subtotal = this.pricesIncludeVat 
                ? calculatedPublicOrder.Total 
                : calculatedPublicOrder.Subtotal;

            cart.SetPricing(subtotal, calculatedPublicOrder.Total, taxes);

            var priceCalculator = new PriceCalculator(this.pricesIncludeVat, this.inclusiveTaxCalculationType);

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

                IEnumerable<Tax> itemTaxes = new List<Tax>();

                if (calculatedItem.Taxes != null && calculatedItem.Taxes.Count() > 0)
                {
                    itemTaxes = calculatedItem.Taxes.Select(t => new Tax(t.Name, calculatedItem.TaxAmount, t.Percent));
                }

                var price = priceCalculator.CalculatePrice(calculatedItem.Price, calculatedItem.Taxes);
                var discount = priceCalculator.CalculatePrice(calculatedItem.Discount, calculatedItem.Taxes);

                cartItem.SetPricing(price, discount, itemTaxes);
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
