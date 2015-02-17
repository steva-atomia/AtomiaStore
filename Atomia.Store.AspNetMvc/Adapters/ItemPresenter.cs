using Atomia.Store.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Adapters
{
    public class ItemPresenter: IItemPresenter
    {
        private readonly IProductProvider productProvider = DependencyResolver.Current.GetService<IProductProvider>();
        private readonly IDomainsProvider domainsProvider = DependencyResolver.Current.GetService<IDomainsProvider>();
        
        public string GetName(IPresentableItem item)
        {
            var product = productProvider.GetProduct(item.ArticleNumber);
            var domainCategories = domainsProvider.GetDomainCategories();

            if (domainCategories.Contains(product.Category))
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
