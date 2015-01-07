/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

Atomia.ViewModels.CartItem = function(cartApi) {
    var theItem = this;

    this.ShouldBeInCart = ko.observable(false);
    this.ShouldBeInCart.subscribe(function (newValue) {
        if (newValue && !_.has(theItem, 'CartItemId')) {
            cartApi.addItem(theItem, undefined, function () {
                theItem.ShouldBeInCart(false);
            });
        }
        else if (_.has(theItem, 'CartItemId')) {
            cartApi.removeItem(theItem, undefined, function () {
                theItem.ShouldBeInCart(true);
            })
        }
    });
};

Atomia.ViewModels.CartItem.prototype.addToCart = function () {
    this.ShouldBeInCart(true);
};

Atomia.ViewModels.CartItem.prototype.removeFromCart = function () {
    this.ShouldBeInCart(false);
};
