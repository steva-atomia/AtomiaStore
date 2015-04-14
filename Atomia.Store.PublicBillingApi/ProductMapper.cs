using Atomia.Store.Core;
using Atomia.Web.Plugin.ProductsProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using ApiProduct = Atomia.Web.Plugin.ProductsProvider.Product;
using CoreProduct = Atomia.Store.Core.Product;

namespace Atomia.Store.PublicBillingApi
{
    /// <summary>
    /// Helpers for mapping between products from Atomia Billing Product Service and AtomiaStore products
    /// </summary>
    internal sealed class ProductMapper
    {
        /// <summary>
        /// Map product from Atomia Billing Product Service to AtomiaStore product
        /// </summary>
        public static CoreProduct Map(ApiProduct apiProduct, Language language, string currencyCode)
        {
            var product = new CoreProduct()
            {
                ArticleNumber = apiProduct.ArticleNumber,
                Category = apiProduct.Category
            };

            SetNameAndDescription(product, apiProduct, language);

            SetPricingVariants(product, apiProduct, currencyCode);

            SetCustomAttributes(product, apiProduct);

            return product;
        }

        /// <summary>
        /// Set localized name or description on product
        /// </summary>
        /// <param name="product">The product to set name and description on</param>
        /// <param name="apiProduct">The Atomia Billing product to get localized values from</param>
        /// <param name="language">The localization langugage to use</param>
        private static void SetNameAndDescription(CoreProduct product, ApiProduct apiProduct, Language language)
        {
            // Set defaults before checking if translations are available.
            product.Name = apiProduct.Name;
            product.Description = apiProduct.Description;

            if (apiProduct.MultilanguageNames != null)
            {
                var names = apiProduct.MultilanguageNames.Where(l => l.LanguageIso639Name.ToUpper() == language.PrimaryTag);
                var regionalName = names.FirstOrDefault(l => l.LanguageCulture.ToUpper() == language.RegionTag);
                var standardName = names.FirstOrDefault();
                
                if (regionalName != null)
                {
                    product.Name = regionalName.Value;
                }
                else if (standardName != null)
                {
                    product.Name = standardName.Value;
                }
            }

            if (apiProduct.MultilanguageDescriptions != null)
            {
                var descriptions = apiProduct.MultilanguageDescriptions.Where(l => l.LanguageIso639Name.ToUpper() == language.PrimaryTag);
                var regionalDescription = descriptions.FirstOrDefault(l => l.LanguageCulture.ToUpper() == language.RegionTag);
                var standardDescription = descriptions.FirstOrDefault();
                
                if (regionalDescription != null)
                {
                    product.Description = regionalDescription.Value;
                }
                else if (standardDescription != null)
                {
                    product.Description = standardDescription.Value;
                }
            }
        }

        /// <summary>
        /// Select and set price in relevant currency
        /// </summary>
        /// <param name="product">The product to set price on</param>
        /// <param name="apiProduct">The Atomia Billing product to select prices from</param>
        /// <param name="language">The currency to use</param>
        private static void SetPricingVariants(CoreProduct product, ApiProduct apiProduct, string currencyCode)
        {
            product.PricingVariants = new List<PricingVariant>();

            if (apiProduct.RenewalPeriods != null && apiProduct.RenewalPeriods.Count > 0)
            {
                foreach (var renewalPeriod in apiProduct.RenewalPeriods)
                {
                    var price = FindPriceValue(renewalPeriod.Prices, currencyCode);
                    
                    product.PricingVariants.Add(new PricingVariant
                    {
                        Price = price,
                        RenewalPeriod = new RenewalPeriod(renewalPeriod.RenewalPeriodValue, renewalPeriod.RenewalPeriodUnit)
                    });
                }
            }
            else
            {
                var price = FindPriceValue(apiProduct.Prices, currencyCode);

                product.PricingVariants.Add(new PricingVariant
                {
                    Price = price,
                    RenewalPeriod = null
                });
            }
        }

        /// <summary>
        /// Copy custom attributes from Atomia Billing product to AtomiaStore product
        /// </summary>
        private static void SetCustomAttributes(CoreProduct product, ApiProduct apiProduct)
        {
            product.CustomAttributes = new List<CustomAttribute>();

            foreach (var prop in apiProduct.Properties)
            {
                product.CustomAttributes.Add(new CustomAttribute
                {
                    Name = prop.Key,
                    Value = prop.Value
                });
            }
        }

        /// <summary>
        /// Find price value for currency among prices.
        /// </summary>
        private static decimal FindPriceValue(IList<ProductPrice> prices, string currencyCode)
        {
            var price = prices.FirstOrDefault(p => p.CurrencyCode == currencyCode);

            if (price == null)
            {
                throw new ArgumentException(String.Format("No prices available for currency code {0}", currencyCode));
            }

            return price.Value;
        }
    }
}
