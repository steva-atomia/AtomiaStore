/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (module, _) {
    'use strict';

    var HostingPackagesItem = function HostingPackagesItem(productData) {
        _.extend(this, productData);
    };

    module.HostingPackagesItem = HostingPackagesItem;

})(Atomia.ViewModels, _);


(function (module, _, ko) {
    'use strict';

    var HostingPackages = function HostingPackages() {
        this.Products = ko.observableArray();

        this.Init = this._Init.bind(this);
        this.UpdateProducts = this._UpdateProducts.bind(this);
        this.Load = this._Load.bind(this);
    };

    HostingPackages.prototype = {
        _Init: function(MakeCartItem, HostingPackagesItem) {
            this._MakeCartItem = MakeCartItem;
            this._HostingPackagesItem = HostingPackagesItem;
        },
        _UpdateProducts: function (products) {
            var self = this;

            _.each(products, function (product) {
                var productToAdd = self._MakeCartItem(new self._HostingPackagesItem(product));

                self.Products.push(productToAdd);
            });
        },

        _Load: function (listProductsDataResponse) {
            this.UpdateProducts(listProductsDataResponse.data.CategoryData.Products);
        }
    };
    
    module.HostingPackages = HostingPackages;

})(Atomia.ViewModels, _, ko);
