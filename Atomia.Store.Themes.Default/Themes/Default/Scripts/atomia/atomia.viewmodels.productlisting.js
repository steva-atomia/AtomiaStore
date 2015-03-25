/// <reference path="../../../../Scripts/underscore.js" />
/// <reference path="../../../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="atomia.utils.js" />
/// <reference path="atomia.viewmodels.cart.js" />

/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils, viewModelsApi) {
    'use strict';

    var ProductListingItemPrototype,
        CreateProductListingItem,
        ProductListingModelPrototype,
        CreateProductListingModel;
    

    /* Products listing item prototype and factory */
    ProductListingItemPrototype = {
        /** Pre-select pricing variant to match the one added to cart. */
        _InitPricingVariant: function _InitPricingVariant() {
            var itemInCart = this.GetItemInCart(),
                selectedPricingVariant = _.find(this.PricingVariants, function (pv) {
                    if (pv.RenewalPeriod === null || itemInCart.RenewalPeriod === null) {
                        return false;
                    }

                    return pv.RenewalPeriod.Unit === itemInCart.RenewalPeriod.Unit &&
                            pv.RenewalPeriod.Period === itemInCart.RenewalPeriod.Period;
                });

            if (selectedPricingVariant !== undefined) {
                this.SelectedPricingVariant(selectedPricingVariant);
            }
        },

        /** Shortcut to price of pricing variant. */
        _Price: function _Price() {
            if (this.HasVariants()) {
                return this.SelectedPricingVariant().Price;
            }

            return this.PricingVariants[0].Price;
        },

        /** Shortcut to renewal period of pricing variant */
        _RenewalPeriod: function _RenewalPeriod() {
            if (this.HasVariants()) {
                return this.SelectedPricingVariant().RenewalPeriod;
            }

            return this.PricingVariants[0].RenewalPeriod;
        }, 

        /** Check if there is more than one pricing variant for the product. */
        _HasVariants: function _HasVariants() {
            return this.PricingVariants.length > 1;
        },

        /** Select pricing variant for product and sync with cart. */
        _SelectPricingVariant: function _SelectPricingVariant() {
            if (this._selectedPricingVariantInitialized && this.IsInCart()) {
                this.RemoveFromCart();
            }

            this._selectedPricingVariantInitialized = true;
        }
    };


    /**
     * Creates product listing item
     * @param {Object|Function} extensions - Extensions to the default product listing item.
     * @param {Object} instance - The instance to create product listing item from.
     */
    CreateProductListingItem = function CreateProductListingItem(extensions, instance) {
        var item, defaults;

        defaults = function (self) {
            return {
                _selectedPricingVariantInitialized: false,

                UniqueId: _.uniqueId('productitem-'),
                SelectedPricingVariant: ko.observable(),

                Price: ko.pureComputed(self._Price, self),
                RenewalPeriod: ko.pureComputed(self._RenewalPeriod, self),
                HasVariants: ko.pureComputed(self._HasVariants, self)
            };
        };

        item = utils.createViewModel(ProductListingItemPrototype, defaults, instance, extensions);
        item.SelectedPricingVariant.subscribe(item._SelectPricingVariant, item);

        return item;
    };



    /* Products listing prototype and factory */
    ProductListingModelPrototype = {
        /** Load view model with product listing data generated on server. */
        Load: function Load(listProductsDataResponse) {
            this._UpdateProducts(listProductsDataResponse.data.CategoryData.Products);
        },

        /** Select product and update cart. */
        SelectProduct: function SelectProduct(item) {
            if (this.SingleSelection) {
                this.SelectedProduct(item);

                _.each(this.Products(), function (product) {
                    if (this._Cart.Contains(product)) {
                        this._Cart.Remove(product, false);
                    }
                }.bind(this));
            }

            this._Cart.Add(item);
        },

        /** Remove product from cart. */
        RemoveProduct: function RemoveProduct(item) {
            this._Cart.Remove(item);
        },

        /** Create and add product listing items from products data. */
        _UpdateProducts: function _UpdateProducts(products) {
            _.each(products, function (product) {
                var item = this.CreateProductListingItem(product);

                item = viewModelsApi.AddCartItemExtensions(this._Cart, item);

                if (this._Cart.Contains(item)) {
                    item._InitPricingVariant();
                    
                    if (this.SingleSelection) {
                        this.SelectProduct(item);
                    }
                }

                this.Products.push(item);
            }, this);
        },

        /** Check if product is in cart. */
        _ProductIsSelected: function _ProductIsSelected() {
            return _.any(this.Products(), function (product) {
                return this._Cart.Contains(product);
            }, this);
        },

        /** Check if order flow is allowed to continue to next step. */
        _AllowContinue: function _AllowContinue() {
            var conditions = [];

            if (this.ProductIsRequired) {
                conditions.push(this.ProductIsSelected());
            }

            return _.every(conditions);
        }
    };

    /**
     * Create product listing.
     * @param {Object} cart - An instance of cart view model.
     * @param {Object|Function} extensions - Extensions to the default product listing view model
     * @param {Object|Function} itemExtensions - Extensions to the default product listing item view model
     */
    CreateProductListingModel = function CreateProductListingModel(cart, extensions, itemExtensions) {
        var defaults = function (self) {
            return {
                _Cart: cart,
                CreateProductListingItem: _.partial(CreateProductListingItem, itemExtensions || {}),

                ProductIsRequired: true,
                SingleSelection: false,
                Products: ko.observableArray(),
                SelectedProduct: ko.observable(),

                ProductIsSelected: ko.pureComputed(self._ProductIsSelected, self),
                AllowContinue: ko.pureComputed(self._AllowContinue, self)
            };
        };

        return utils.createViewModel(ProductListingModelPrototype, defaults, extensions);
    };
    

    _.extend(exports, {
        CreateProductListingModel: CreateProductListingModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils, Atomia.ViewModels);
