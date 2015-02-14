using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Models
{
    public class ProductModel : IPresentableItem
    {
        private readonly IItemPresenter itemPresenter = DependencyResolver.Current.GetService<IItemPresenter>();
        private readonly Product product;

        public ProductModel(Product product)
        {
            this.product = product;
        }

        public string ArticleNumber
        {
            get
            {
                return product.ArticleNumber;
            }
        }

        public string Name
        {
            get
            {
                return itemPresenter.GetName(this);
            }
        }

        public string Description
        {
            get
            {
                return itemPresenter.GetDescription(this);
            }
        }

        public List<CustomAttribute> CustomAttributes
        {
            get
            {
                if (product.CustomAttributes == null)
                {
                    product.CustomAttributes = new List<CustomAttribute>();
                }

                return product.CustomAttributes;
            }
        }

        public IReadOnlyCollection<PricingVariantModel> PricingVariants
        {
            get
            {
                return product.PricingVariants.Select(p => new PricingVariantModel(p)).ToList();
            }
        }
    }
}
