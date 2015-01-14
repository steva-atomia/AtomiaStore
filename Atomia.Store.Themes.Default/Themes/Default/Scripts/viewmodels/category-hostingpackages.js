/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */


(function (module, _, ko) {
    'use strict';

    var HostingPackagesItem, HostingPackages;

    HostingPackagesItem = function HostingPackagesItem(productData) {
        _.extend(this, productData);

        this.UniqueId = _.uniqueId('productitem-');
        this.SelectedPricingVariant = ko.observable();
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

        _.bindAll(this, 'Init', '_UpdateProducts', 'Load');
    };

    HostingPackages.prototype = {
        Init: function(cart) {
            this._MakeCartItem = cart.MakeCartItem;
        },

        _UpdateProducts: function (products) {
            var self = this;

            _.each(products, function (product) {
                var productToAdd = self._MakeCartItem(new self.HostingPackagesItem(product));

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
