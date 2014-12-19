using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core
{
    public class ProductSearchQuery
    {
        public List<SearchTerm> Terms { get; set; }
    }

    public class BillingProductsProvider : IProductsProvider
    {

        public IEnumerable<Product> GetProducts(ProductSearchQuery searchQuery)
        {
            // Returning from billing API based on searchQuery.Name == "Category"
            return new List<Product>
            {
                new Product
                {
                    ArticleNumber = "Bloop",
                    CustomAttributes = new List<CustomAttribute>(),
                    PricingVariants = new List<PricingVariant>()
                    {
                        new PricingVariant
                        {
                            Price = 10m,
                            RenewalPeriod = new RenewalPeriod
                            {
                                Period = 1,
                                Unit = "YEAR"
                            }
                        }
                    }
                }
            };
        }
    }

    public class AddonsProvider : IProductsProvider
    {
        private readonly IProductsProvider billingProvider;
        private readonly ICartProvider cartProvider;

        public AddonsProvider(IProductsProvider productsProvider, ICartProvider cartProvider)
        {
            billingProvider = productsProvider;
            this.cartProvider = cartProvider;
        }

        public IEnumerable<Product> GetProducts(ProductSearchQuery searchQuery)
        {
            var products = billingProvider.GetProducts(searchQuery);
            var cart = cartProvider.GetCart();

            return products.Where(
                p => p.Category == "Extra service" && p.CustomAttributes.Any(ca => ca.Name == "AddonFor" && cart.CartItems.Select(ci => ci.ArticleNumber).Contains(ca.Value)));
        }
    }
}
