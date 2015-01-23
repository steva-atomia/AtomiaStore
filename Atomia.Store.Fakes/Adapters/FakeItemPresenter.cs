using Atomia.Store.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakeItemPresenter: IItemPresenter
    {
        private List<Product> allProducts;

        public FakeItemPresenter()
        {
            // TODO: for real presenter use plugin product provider interface for getting based on articlenumber
            var productsProvider = new FakeCategoryProductsProvider();

            this.allProducts = productsProvider.GetAllProducts().ToList();
        }
        
        public string GetName(IPresentableItem item)
        {
            var product = allProducts.First(p => p.ArticleNumber == item.ArticleNumber);

            if (product.Category == "Domain")
            {
                var domainNameAttr = item.CustomAttributes.FirstOrDefault(ca => ca.Name == "DomainName");
                
                if (domainNameAttr != default(CustomAttribute)) {
                    return domainNameAttr.Value;
                }
            }

            return product.Name;
        }

        public string GetDescription(IPresentableItem item)
        {
            var product = allProducts.First(p => p.ArticleNumber == item.ArticleNumber);

            return product.Description;
        }

        public string GetCategory(IPresentableItem item)
        {
            var product = allProducts.First(p => p.ArticleNumber == item.ArticleNumber);

            return product.Category;
        }
    }
}
