/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

Atomia.ViewModels.Cart = function (_, ko, amplify, cartApi) {
    'use strict';

    var CartItems = ko.observableArray(),
        SubTotal = ko.observable(0),
        Total = ko.observable(0),
        Tax = ko.observable(0),
        IsOpen = ko.observable(false),
        NumberOfItems = ko.pureComputed(function () {
            return CartItems().length;
        }),
        IsEmpty = ko.pureComputed(function () {
            return NumberOfItems() <= 0;
        });

    function ToggleDropdown() {
        IsOpen() ? IsOpen(false) : IsOpen(true);
    }

    amplify.subscribe('cart:updated', function () {
        CartItems.removeAll();

        _.each(cartApi.getCartItems(), function (cartItem) {
            var cartItemViewModel = cartApi.createCartItemViewModel({
                init: function (item) {
                    _.extend(item, cartItem);
                }
            });

            CartItems.push(cartItemViewModel);
        });

        SubTotal(cartApi.getSubTotal());
        Total(cartApi.getTotal());
        Tax(cartApi.getTax());
    });

    return {
        CartItems: CartItems,
        SubTotal: SubTotal,
        Total: Total,
        Tax: Tax,
        NumberOfItems: NumberOfItems,
        IsOpen: IsOpen,
        IsEmpty: IsEmpty,
        ToggleDropdown: ToggleDropdown
    };
};


if (Atomia.RootVM !== undefined) {
    Atomia.RootVM.Cart = Atomia.ViewModels.Cart(_, ko, amplify, Atomia.Cart);
}
