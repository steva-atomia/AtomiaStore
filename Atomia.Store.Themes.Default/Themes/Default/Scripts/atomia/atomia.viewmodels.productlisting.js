/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils, viewModelsApi) {
    'use strict';

    /**
     * Creates product listing item
     * @param {Object} productData - The instance to create product listing item from.
     */
    function ProductListingItem(productData) {
        var self = this;

        self._selectedPricingVariantInitialized = false;
        
        self.uniqueId = _.uniqueId('productitem-');
        self.selectedPricingVariant = ko.observable();
        
        /** Shortcut to price of pricing variant. */
        self.price = ko.pureComputed(function() {
            if (self.hasVariants()) {
                return self.selectedPricingVariant().Price;
            }

            return self.PricingVariants[0].Price;
        });

        /** Shortcut to renewal period of pricing variant */
        self.renewalPeriod = ko.pureComputed(function() {
            if (self.hasVariants()) {
                return self.selectedPricingVariant().RenewalPeriod;
            }

            return self.PricingVariants[0].RenewalPeriod;
        });

        // TODO: Remove this when change to using all lower case is done.
        self.Price = self.price;
        self.RenewalPeriod = self.renewalPeriod;

        /** Check if there is more than one pricing variant for the product. */
        self.hasVariants = ko.pureComputed(function () {
            return self.PricingVariants.length > 1;
        });
            
        /** Pre-select pricing variant to match the one added to cart. */
        self.initPricingVariant = function initPricingVariant() {
            var itemInCart = self.getItemInCart(),
                selectedPricingVariant = _.find(self.PricingVariants, function (pv) {
                    if (pv.RenewalPeriod === null || itemInCart.RenewalPeriod === null) {
                        return false;
                    }

                    return pv.RenewalPeriod.Unit === itemInCart.RenewalPeriod.Unit &&
                            pv.RenewalPeriod.Period === itemInCart.RenewalPeriod.Period;
                });

            if (selectedPricingVariant !== undefined) {
                self.selectedPricingVariant(selectedPricingVariant);
            }
        };
        
        /** Select pricing variant for product and sync with cart. */
        self.selectedPricingVariant.subscribe(function _SelectPricingVariant() {
            if (self._selectedPricingVariantInitialized && self.isInCart()) {
                self.removeFromCart();
            }

            self._selectedPricingVariantInitialized = true;
        });

        /** Add properties from productData to object */
        _.extend(self, productData);
    }


    /**
     * Create product listing.
     * @param {Object} cart - An instance of cart view model.
     */
    function ProductListingModel(cart) {
        var self = this;
                
        self.productIsRequired = true;
        self.singleSelection = false;
        self.products = ko.observableArray();
        self.selectedProduct = ko.observable();
        
        /** Check if product is in cart. */
        self.productIsSelected = ko.pureComputed(function() {
            return _.any(self.products(), function (product) {
                return cart.contains(product);
            });
        });

        /** Check if order flow is allowed to continue to next step. */
        self.allowContinue = ko.pureComputed(function() {
            var conditions = [];

            if (self.productIsRequired) {
                conditions.push(self.productIsSelected());
            }

            return _.every(conditions);
        });
          
        /** Create ProductListingItem object. */
        self.createProductListingItem = function CreateProductListingItem(productData){
            return new ProductListingItem(productData);
        };

        /** Select product and update cart. */
        self.selectProduct = function SelectProduct(item) {
            if (self.singleSelection) {
                self.selectedProduct(item);

                _.each(self.products(), function (product) {
                    if (cart.contains(product)) {
                        cart.remove(product, false);
                    }
                });
            }

            cart.add(item);
        };

        /** Remove product from cart. */
        self.removeProduct = function RemoveProduct(item) {
            cart.remove(item);
        };

        /** Load view model with product listing data generated on server. */
        self.load = function load(response) {
            var products = response.data.CategoryData.Products;

            _.each(products, function (product) {
                var item = self.createProductListingItem(product);

                item = viewModelsApi.addCartItemExtensions(cart, item);

                if (cart.contains(item)) {
                    item.initPricingVariant();
                    
                    if (self.singleSelection) {
                        self.selectProduct(item);
                    }
                }

                self.products.push(item);
            });
        };
    }
    

    _.extend(exports, {
        ProductListingItem: ProductListingItem,
        ProductListingModel: ProductListingModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils, Atomia.ViewModels);
