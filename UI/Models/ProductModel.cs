using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Atomia.Store.Core;
using Atomia.Store.AspNetMvc.Services;

namespace Atomia.Store.AspNetMvc.Models
{
    public class ProductModel : IPresentableItem
    {
        private readonly IItemPresenter itemPresenter;
        private readonly Product product;

        public ProductModel(Product product)
        {
            this.itemPresenter = DependencyResolver.Current.GetService<IItemPresenter>();
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
