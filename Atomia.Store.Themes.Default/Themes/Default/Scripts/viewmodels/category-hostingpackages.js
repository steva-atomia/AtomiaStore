/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */


(function (module, _, ko) {
    'use strict';

    var ProductsListingItem,
        ProductsListing;


    /* SingleSelectionDomainProductsItem and prototype */
    ProductsListingItem = function ProductsListingItem(productData) {
        this._selectedPricingVariantInitialized = false;

        _.extend(this, productData);

        this.UniqueId = _.uniqueId('productitem-');

        this.SelectedPricingVariant = ko.observable();
        this.SelectedPricingVariant.subscribe(this._SelectPricingVariant, this);

        this.Price = ko.pureComputed(this._Price, this);
        this.RenewalPeriod = ko.pureComputed(this._RenewalPeriod, this);
        this.HasVariants = ko.pureComputed(this._HasVariants, this);

        _.bindAll(this, '_InitPricingVariant');
    };

    ProductsListingItem.prototype = {
        _SelectPricingVariant: function () {
            if (this._selectedPricingVariantInitialized && this.IsInCart()) {
                this.RemoveFromCart();
            }

            this._selectedPricingVariantInitialized = true;
        },

        _Price: function () {
            if (this.HasVariants()) {
                return this.SelectedPricingVariant().Price;
            }

            return this.PricingVariants[0].Price;
        },

        _RenewalPeriod: function () {
            if (this.HasVariants()) {
                return this.SelectedPricingVariant().RenewalPeriod;
            }

            return this.PricingVariants[0].RenewalPeriod;
        },

        _HasVariants: function () {
            return this.PricingVariants.length > 1;
        },

        _InitPricingVariant: function () {
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
        }
    };


    /* SingleSelectionDomainProducts and prototype */
    ProductsListing = function ProductsListing() {

        this.Item = ProductsListingItem;
        this._Options = {
            ProductIsRequired: true,
            SingleSelection: false
        };

        this.ProductIsRequired = ko.pureComputed(this._ProductIsRequired, this);
        this.SingleSelection = ko.pureComputed(this._SingleSelection, this);

        this.Products = ko.observableArray();
        this.SelectedProduct = ko.observable();
        this.ProductIsSelected = ko.pureComputed(this._ProductIsSelected, this);
        
        this.AllowContinue = ko.pureComputed(this._AllowContinue, this);
        
        _.bindAll(this, 'Init', 'Load', 'SelectProduct', 'RemoveProduct', '_UpdateProducts');
    };

    ProductsListing.prototype = {
        Init: function (cart, options) {
            this._Cart = cart;

            options = options || {};
            _.extend(this._Options, options);
        },

        Load: function (listProductsDataResponse) {
            this._UpdateProducts(listProductsDataResponse.data.CategoryData.Products);
        },

        SelectProduct: function (item) {
            if (this.SingleSelection()) {
                this.SelectedProduct(item);

                _.each(this.Products(), function (product) {
                    if (this._Cart.Contains(product)) {
                        this._Cart.Remove(product);
                    }
                }.bind(this));
            }

            this._Cart.Add(item);
        },

        RemoveProduct: function(item) {
            this._Cart.Remove(item);
        },

        _SingleSelection: function() {
            return this._Options.SingleSelection;
        },

        _ProductIsRequired: function () {
            return this._Options.ProductIsRequired;
        },

        _ProductIsSelected: function () {
            return _.any(this.Products(), function (product) {
                return this._Cart.Contains(product);
            }, this);
        },

        _UpdateProducts: function (products) {
            _.each(products, function (product) {
                var item = new this.Item(product);

                item = this._Cart.ExtendWithCartProperties(item);

                if (this._Cart.Contains(item)) {
                    item._InitPricingVariant();
                    
                    if (this.SingleSelection()) {
                        this.SelectProduct(item);
                    }
                }

                this.Products.push(item);
            }, this);
        },

        _AllowContinue: function () {
            var conditions = [];

            if (this.ProductIsRequired()) {
                conditions.push(this.ProductIsSelected());
            }

            return _.every(conditions);
        }
    };
    


    /* Export models */
    module.ProductsListingItem = ProductsListingItem;
    module.ProductsListing = ProductsListing;

})(Atomia.ViewModels, _, ko);
