/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

Atomia.ViewModels.HostingPackages = function (_, ko, cart) {
    'use strict';

    var Products = ko.observableArray();

    function Product(productData) {
        _.extend(this, productData);
    }

    function _updateProducts(products) {
        _.each(products, function (product) {
            var productToAdd = cart.MakeCartItem(new Product(product));

            Products.push(productToAdd);
        });
    }

    function LoadProducts(listProductsDataResponse) {
        _updateProducts(listProductsDataResponse.data.CategoryData.Products);
    }
    
    return {
        Products: Products,
        LoadProducts: LoadProducts
    };
};

if (Atomia.ViewModels.Active !== undefined) {
    Atomia.ViewModels.Active.HostingPackages = Atomia.ViewModels.HostingPackages(_, ko, Atomia.ViewModels.Active.Cart);
}
