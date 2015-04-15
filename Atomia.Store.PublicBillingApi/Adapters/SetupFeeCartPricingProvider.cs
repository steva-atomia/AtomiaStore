using Atomia.Store.Core;
using Atomia.Web.Plugin.ProductsProvider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    /// <summary>
    /// Decorator for the basic <see cref="CartPricingProvider"/> (or other basic provider). 
    /// It checks the cart for products that should have setup fee added and adds it to the cart if any are found,
    /// and then lets the base <see cref="CartPricingProvider"/> calculate the cart.
    /// </summary>
    public class SetupFeeCartPricingService : ICartPricingService
    {
        private readonly ICartPricingService baseCartPricingService;
        private readonly ApiProductsProvider apiProductsProvider;

        /// <summary>
        /// The product category of the setup fee.
        /// </summary>
        protected virtual string SetupFeeCategory
        {
            get { return "SetupFee"; }
        }

        /// <summary>
        /// The product categories that should have setup fee added.
        /// </summary>
        protected virtual IEnumerable<string> ProductCategoriesWithSetupFee
        {
            get { return new string[] { "HostingPackage" }; }
        }

        /// <summary>
        /// Create new instance wrapping the base service.
        /// </summary>
        public SetupFeeCartPricingService(ICartPricingService baseCartPricingService, ApiProductsProvider apiProductsProvider)
        {
            if (baseCartPricingService == null)
            {
                throw new ArgumentNullException("baseCartPricingService");
            }

            if (apiProductsProvider == null)
            {
                throw new ArgumentNullException("apiProductsProvider");
            }

            this.baseCartPricingService = baseCartPricingService;
            this.apiProductsProvider = apiProductsProvider;
        }

        /// <summary>
        /// Add or remove any required setup fees before calculating pricing for cart. 
        /// Setup fee items are marked with "NotRemovable" custom attributes as help to e.g. GUI code.
        /// </summary>
        public Cart CalculatePricing(Cart cart)
        {
            var setupFee = apiProductsProvider.GetProductsByCategories(new List<string>() { SetupFeeCategory }).FirstOrDefault();
            
            if (setupFee != null)
            {
                var setupFeeProducts = apiProductsProvider.GetProductsByCategories(ProductCategoriesWithSetupFee.ToList());

                var shouldHaveSetupFee = setupFeeProducts.Any(p => cart.CartItems.Any(ci => ci.ArticleNumber == p.ArticleNumber));
                var hasSetupFee = cart.CartItems.Any(ci => ci.ArticleNumber == setupFee.ArticleNumber);

                if (shouldHaveSetupFee && !hasSetupFee)
                {
                    var setupFeeItem = new CartItem
                    {
                        ArticleNumber = setupFee.ArticleNumber,
                        Quantity = 1,
                        RenewalPeriod = null,
                        CustomAttributes = new List<Core.CustomAttribute>()
                        {
                            new Core.CustomAttribute{ Name = "NotRemovable", Value = "true" }
                        }
                    };

                    cart.AddItem(setupFeeItem);
                }
                else if (!shouldHaveSetupFee && hasSetupFee)
                {
                    var setupFeeItem = cart.CartItems.First(ci => ci.ArticleNumber == setupFee.ArticleNumber);
                    
                    cart.RemoveItem(setupFeeItem.Id);
                }
            }
            
            var calculatedCart = baseCartPricingService.CalculatePricing(cart);

            return calculatedCart;
        }
    }
}
