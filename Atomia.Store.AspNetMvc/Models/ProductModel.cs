using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// A view model of a <see cref="Atomia.Store.Core.Product"/> with added localized name and description.
    /// </summary>
    public class ProductModel : IPresentableItem
    {
        private readonly IItemPresenter itemPresenter = DependencyResolver.Current.GetService<IItemPresenter>();
        private readonly Product product;

        /// <summary>
        /// Construct the view model from the <see cref="Atomia.Store.Core.Product"/>
        /// </summary>
        public ProductModel(Product product)
        {
            this.product = product;
        }

        /// <summary>
        /// The article number of the underlying product.
        /// </summary>
        public string ArticleNumber
        {
            get
            {
                return product.ArticleNumber;
            }
        }

        /// <summary>
        /// Localized name of the product
        /// </summary>
        public string Name
        {
            get
            {
                return itemPresenter.GetName(this);
            }
        }

        /// <summary>
        /// Localized description of the product
        /// </summary>
        public string Description
        {
            get
            {
                return itemPresenter.GetDescription(this);
            }
        }

        /// <summary>
        /// Custom attributes from the underlying product
        /// </summary>
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

        /// <summary>
        /// Display formatted renewal periods and prices from the underlying product.
        /// </summary>
        public IReadOnlyCollection<PricingVariantModel> PricingVariants
        {
            get
            {
                return product.PricingVariants.Select(p => new PricingVariantModel(p)).ToList();
            }
        }
    }
}
