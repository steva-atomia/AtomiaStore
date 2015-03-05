using Atomia.Store.Core;
using Atomia.Web.Plugin.ProductsProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using ApiProduct = Atomia.Web.Plugin.ProductsProvider.Product;
using CoreProduct = Atomia.Store.Core.Product;

namespace Atomia.Store.PublicBillingApi
{
    internal sealed class ProductMapper
    {
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
                    product.Name = regionalDescription.Value;
                }
                else if (standardDescription != null)
                {
                    product.Name = standardDescription.Value;
                }
            }
        }

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
