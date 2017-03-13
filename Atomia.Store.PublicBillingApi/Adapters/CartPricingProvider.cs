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
        private readonly IContactDataProvider contactDataProvider;
        private readonly RenewalPeriodProvider renewalPeriodProvider;
        private readonly IVatNumberValidator vatNumberValidator;
        private readonly bool pricesIncludeVat;
        private readonly bool inclusiveTaxCalculationType;

        /// <summary>
        /// Construct a new instance of CartPricingProvider
        /// </summary>
        public CartPricingProvider(
            IResellerProvider resellerProvider, 
            ICurrencyPreferenceProvider currencyPreferenceProvider, 
            ICountryProvider countryProvider, 
            IContactDataProvider contactDataProvider,
            RenewalPeriodProvider renewalPeriodProvider, 
            IVatDisplayPreferenceProvider vatDisplayPreferenceProvider, 
            PublicBillingApiProxy billingApi,
            IVatNumberValidator vatNumberValidator)
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

            if (contactDataProvider == null)
            {
                throw new ArgumentNullException("contactDataProvider");
            }

            if (renewalPeriodProvider == null)
            {
                throw new ArgumentNullException("renewalPeriodProvider");
            }

            if (vatDisplayPreferenceProvider == null)
            {
                throw new ArgumentNullException("vatDisplayPreferenceProvider");
            }

            if (vatNumberValidator == null)
            {
                throw new ArgumentNullException("vatNumberValidator");
            }

            this.resellerProvider = resellerProvider;
            this.currencyPreferenceProvider = currencyPreferenceProvider;
            this.countryProvider = countryProvider;
            this.contactDataProvider = contactDataProvider;
            this.renewalPeriodProvider = renewalPeriodProvider;
            this.pricesIncludeVat = vatDisplayPreferenceProvider.ShowPricesIncludingVat();
            this.inclusiveTaxCalculationType = resellerProvider.GetReseller().InclusiveTaxCalculationType;
            this.vatNumberValidator = vatNumberValidator;
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
                    });
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
            string country = this.countryProvider.GetDefaultCountry().Code;
            var vatValidationResult = vatNumberValidator.ValidateCustomerVatNumber();
            var vatNumber = string.Empty;
            

            if (vatValidationResult != null && vatValidationResult.Valid)
            {
                vatNumber = vatValidationResult.VatNumber;
                country = vatValidationResult.CountryCode;
            }
            else if (this.contactDataProvider != null)
            {
                var allContacts = this.contactDataProvider.GetContactData();
                IEnumerable<ContactData> contacts = allContacts?.GetContactData();
                if (contacts != null)
                {
                    ContactData contactData = contacts.FirstOrDefault(c => c.Id == "BillingContact" && !string.IsNullOrEmpty(c.Country))
                                              ?? contacts.FirstOrDefault(c => c.Id == "MainContact" && !string.IsNullOrEmpty(c.Country));
                    if (!string.IsNullOrEmpty(contactData?.Country))
                    {
                        country = contactData.Country;
                    }
                }
            }
            
            return new PublicOrder
            {
                ResellerId = resellerProvider.GetReseller().Id,
                Country = country,
                Currency = currencyPreferenceProvider.GetCurrentCurrency().Code,
                LegalNumber = vatNumber
            };
        }
    }
}
