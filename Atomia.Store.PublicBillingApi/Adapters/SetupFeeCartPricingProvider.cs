using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System.Collections.Generic;
using System.Linq;
using System;
using Atomia.Web.Plugin.ProductsProvider;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    public class SetupFeeCartPricingService : ICartPricingService
    {
        private readonly ICartPricingService baseCartPricingService;
        private readonly IResellerProvider resellerProvider;
        private readonly IProductsProvider productsProvider;


        protected virtual string SetupFeeCategory
        {
            get { return "SetupFee"; }
        }

        protected virtual IEnumerable<string> ProductCategoriesWithSetupFee
        {
            get { return new string[] { "HostingPackage" }; }
        }


        public SetupFeeCartPricingService(ICartPricingService baseCartPricingService, IResellerProvider resellerProvider, IProductsProvider productsProvider)
        {
            if (baseCartPricingService == null)
            {
                throw new ArgumentNullException("baseCartPricingService");
            }

            if (resellerProvider == null)
            {
                throw new ArgumentNullException("resellerProvider");
            }

            if (productsProvider == null)
            {
                throw new ArgumentNullException("productsProvider");
            }

            this.baseCartPricingService = baseCartPricingService;
            this.resellerProvider = resellerProvider;
            this.productsProvider = productsProvider;
        }

        public Cart CalculatePricing(Cart cart)
        {
            var resellerId = resellerProvider.GetReseller().Id;
            var setupFee = productsProvider.GetShopProductsByCategories(resellerId, null, new List<string>() { SetupFeeCategory }).FirstOrDefault();
            
            if (setupFee != null)
            {
                var setupFeeProducts = productsProvider.GetShopProductsByCategories(resellerId, null, ProductCategoriesWithSetupFee.ToList());

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
