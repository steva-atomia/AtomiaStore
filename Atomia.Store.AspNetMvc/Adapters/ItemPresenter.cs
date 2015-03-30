using Atomia.Store.Core;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Adapters
{
    /// <summary>
    /// <see cref="Atomia.Store.Core.IItemPresenter"/> that uses default backend values, except for names of domain related products.
    /// </summary>
    public sealed class ItemPresenter : IItemPresenter
    {
        private readonly IProductProvider productProvider = DependencyResolver.Current.GetService<IProductProvider>();
        private readonly IDomainsProvider domainsProvider = DependencyResolver.Current.GetService<IDomainsProvider>();
        
        /// <summary>
        /// Get actual domain name if available for domain products (registration, transfer), 
        /// otherwise get default default name from <see cref="Atomia.Store.Core.IProductProvider"/>
        /// </summary>
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

        /// <summary>
        /// Get default description from <see cref="Atomia.Store.Core.IProductProvider"/>
        /// </summary>
        public string GetDescription(IPresentableItem item)
        {
            var product = productProvider.GetProduct(item.ArticleNumber);

            return product.Description;
        }

        /// <summary>
        /// Get default category from <see cref="Atomia.Store.Core.IProductProvider"/>
        /// </summary>
        public string GetCategory(IPresentableItem item)
        {
            var product = productProvider.GetProduct(item.ArticleNumber);

            return product.Category;
        }
    }
}
