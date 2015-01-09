/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

Atomia.ViewModels.Cart = function (_, ko, cartApi) {
    'use strict';

    var CartItems = ko.observableArray(),
        SubTotal = ko.observable(0),
        Total = ko.observable(0),
        Tax = ko.observable(0),
        CampaignCode = ko.observable(''),
        IsOpen = ko.observable(false),
        NumberOfItems = ko.pureComputed(function () {
            return CartItems().length;
        }),
        IsEmpty = ko.pureComputed(function () {
            return NumberOfItems() <= 0;
        });

    function _updateCart(cartData) {
        CartItems.removeAll();

        _.each(cartData.CartItems, function (cartItem) {
            var itemToAdd = CreateCartItem({
                init: function (item) {
                    _.extend(item, cartItem);

                    item.CartItemId = item.Id;
                },
                equals: function (i1, i2) {
                    return i1.CartItemId === i2.CartItemId;
                }
            });
            CartItems.push(itemToAdd);
        });

        SubTotal(cartData.SubTotal);
        Total(cartData.Total);
        Tax(cartData.Tax);
        CampaignCode(cartData.CampaignCode);
    }

    function ToggleDropdown() {
        IsOpen() ? IsOpen(false) : IsOpen(true);
    }

    function CreateCartItem(options) {
        var item = {};

        options = options || {};

        _.defaults(options, {
            init: _.noop,
            equals: function (i1, i2) {
                return i1.ArticleNumber === i2.ArticleNumber;
            }
        });

        options.init(item);

        item.IsInCart = ko.computed(function () {
            var isInCart = _.any(CartItems(), function (cartItem) {
                return options.equals(item, cartItem);
            })

            return isInCart;
        }).extend({ notify: 'always' });

        item.AddToCart = function () {
            if (!item.IsInCart()) {
                // Preliminarily add item to cart.
                CartItems.push(item);

                cartApi.AddItem(
                    item,
                    function (result) {
                        _updateCart(result.Cart);
                    },
                    function (result) {
                        // Failed: remove item
                        CartItems.remove(function (cartItem) {
                            options.equals(item, cartItem);
                        });
                    }
                );
            }
        };

        item.RemoveFromCart = function () {
            var itemInCart = _.find(CartItems(), function (cartItem) {
                return options.equals(item, cartItem);
            });

            if (itemInCart !== undefined) {
                // Preliminarily remove item from cart.
                CartItems.remove(itemInCart);

                cartApi.RemoveItem(
                    itemInCart,
                    function (result) {
                        _updateCart(result.Cart);
                    },
                    function (result) {
                        // Failed: add back item.
                        CartItems.push(itemInCart);
                    }
                );
            }
        };

        item.ToggleInCart = function () {
            item.IsInCart() ? item.RemoveFromCart() : item.AddToCart();
        }

        return item;
    }

    return {
        CartItems: CartItems,
        SubTotal: SubTotal,
        Total: Total,
        Tax: Tax,
        NumberOfItems: NumberOfItems,
        IsOpen: IsOpen,
        IsEmpty: IsEmpty,
        ToggleDropdown: ToggleDropdown,
        CreateCartItem: CreateCartItem
    };
};


if (Atomia.ViewModels.Active !== undefined) {
    Atomia.ViewModels.Active.Cart = Atomia.ViewModels.Cart(_, ko, Atomia.Cart);
}
