using Atomia.Store.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakeItemPresenter: IItemPresenter
    {
        private List<Product> allProducts;

        public FakeItemPresenter(AllProductsProvider productsProvider)
        {
            if (productsProvider == null)
            {
                throw new ArgumentNullException("productsProvider");
            }

            this.allProducts = productsProvider.GetProducts(null).ToList();
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
