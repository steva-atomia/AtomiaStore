using Atomia.Store.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakeItemPresenter: IItemPresenter
    {
        private IProductProvider productProvider;

        public FakeItemPresenter(IProductProvider productProvider)
        {
            this.productProvider = productProvider;
        }
        
        public string GetName(IPresentableItem item)
        {
            var product = productProvider.GetProduct(item.ArticleNumber);

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
            var product = productProvider.GetProduct(item.ArticleNumber);

            return product.Description;
        }

        public string GetCategory(IPresentableItem item)
        {
            var product = productProvider.GetProduct(item.ArticleNumber);

            return product.Category;
        }
    }
}
