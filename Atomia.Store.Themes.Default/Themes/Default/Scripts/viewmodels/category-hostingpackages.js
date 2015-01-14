/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */


(function (module, _, ko) {
    'use strict';

    var HostingPackagesItem, HostingPackages;

    HostingPackagesItem = function HostingPackagesItem(productData) {
        var selectedPricingVariantInitialized = false;

        _.extend(this, productData);

        this.UniqueId = _.uniqueId('productitem-');
        this.SelectedPricingVariant = ko.observable();

        this.SelectedPricingVariant.subscribe(function (newValue) {
            if (selectedPricingVariantInitialized && this.IsInCart()) {
                this.RemoveFromCart();
            }

            selectedPricingVariantInitialized = true;
        }, this);

        this.Price = ko.pureComputed(function () {
            if (this.HasVariants()) {
                return this.SelectedPricingVariant().Price;
            }

            return this.PricingVariants[0].Price;
        }, this);

        this.RenewalPeriod = ko.pureComputed(function () {
            if (this.HasVariants()) {
                return this.SelectedPricingVariant().RenewalPeriod;
            }

            return this.PricingVariants[0].RenewalPeriod;
        }, this);

        this.HasVariants = ko.pureComputed(function () {
            return this.PricingVariants.length > 1;
        }, this);
    };

    HostingPackages = function HostingPackages() {
        this.HostingPackagesItem = HostingPackagesItem;

        this.Products = ko.observableArray();

        _.bindAll(this, 'Init', '_UpdateProducts', 'Load', '_InitPricingVariant', '_AmendAddForSingleSelection');
    };

    HostingPackages.prototype = {
        Init: function(cart, options) {
            this._MakeCartItem = cart.MakeCartItem;

            this._Options = options || {};
            _.defaults(this._Options, {
                SingleSelection: false
            });
        },

        _InitPricingVariant: function (productToAdd) {
            var itemInCart = productToAdd.GetItemInCart(),
                selectedPricingVariant = _.find(productToAdd.PricingVariants, function (pv) {
                    if (pv.RenewalPeriod === null || itemInCart.RenewalPeriod === null) {
                        return false;
                    }

                    return pv.RenewalPeriod.Unit === itemInCart.RenewalPeriod.Unit &&
                           pv.RenewalPeriod.Period === itemInCart.RenewalPeriod.Period;
                });

            if (selectedPricingVariant !== undefined) {
                productToAdd.SelectedPricingVariant(selectedPricingVariant);
            }
        },

        _AmendAddForSingleSelection: function(productToAdd) {
            var self = this,
                originalAddToCart = productToAdd.AddToCart.bind(productToAdd);

            // Amend to remove any other selected package when adding to cart.
            productToAdd.AddToCart = function () {
                _.invoke(self.Products(), 'RemoveFromCart');

                originalAddToCart();
            }.bind(productToAdd);
        },

        _UpdateProducts: function (products) {
            var self = this;

            _.each(products, function (product) {
                var productToAdd = self._MakeCartItem(new self.HostingPackagesItem(product));
                
                if (self._Options.SingleSelection) {
                    self._AmendAddForSingleSelection(productToAdd);
                }

                if (productToAdd.IsInCart()){
                    self._InitPricingVariant(productToAdd);
                }

                self.Products.push(productToAdd);
            });
        },

        Load: function (listProductsDataResponse) {
            this._UpdateProducts(listProductsDataResponse.data.CategoryData.Products);
        }
    };
    
    module.HostingPackagesItem = HostingPackagesItem;
    module.HostingPackages = HostingPackages;

})(Atomia.ViewModels, _, ko);
