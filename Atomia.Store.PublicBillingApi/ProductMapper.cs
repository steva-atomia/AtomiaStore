using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;
using Atomia.Web.Plugin.ProductsProvider;
using CoreProduct = Atomia.Store.Core.Product;
using ApiProduct = Atomia.Web.Plugin.ProductsProvider.Product;

namespace Atomia.Store.PublicBillingApi
{
    class ProductMapper
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
            // TODO: Update when there is language code available.
            /*if (apiProduct.MultilanguageNames != null && apiProduct.MultilanguageNames.Count > 0)
            {

            }

            if (apiProduct.MultilanguageNames != null && apiProduct.MultilanguageNames.Count > 0)
            {

            }*/

            product.Name = apiProduct.Name;
            product.Description = apiProduct.Description;
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
