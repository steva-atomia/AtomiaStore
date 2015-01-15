using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.AspNetMvc.Providers
{
    public class SimpleCategoryProvider : CategoryProvider
    {
        protected readonly AllProductsProvider packagesProvider;

        public SimpleCategoryProvider(AllProductsProvider packagesProvider)
        {
            this.packagesProvider = packagesProvider;
        }

        public override IEnumerable<Product> GetProducts(ProductSearchQuery searchQuery)
        {
            var categoryTerm = searchQuery.Terms.FirstOrDefault(t => t.Key == "category");

            if (categoryTerm != null)
            {
                var products = packagesProvider.GetProducts(searchQuery);

                return products.Where(p => p.Category == categoryTerm.Value);
            }

            return new List<Product>();
        }
    }
}