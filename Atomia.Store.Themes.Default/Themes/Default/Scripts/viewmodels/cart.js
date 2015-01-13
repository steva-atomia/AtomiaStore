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

    function Item(itemData) {
        _.extend(this, itemData);

        this.CartItemId = this.Id;
        this.Equals = this._Equals.bind(this);
    }

    Item.prototype._Equals = function (other) {
        return this.CartItemId === other.CartItemId;
    }

    function _updateCart(cartData) {
        CartItems.removeAll();

        _.each(cartData.CartItems, function (cartItemData) {
            var item = new Item(cartItemData),
                cartItem = CreateCartItem(item);

            CartItems.push(cartItem);
        });

        SubTotal(cartData.SubTotal);
        Total(cartData.Total);
        Tax(cartData.Tax);
        CampaignCode(cartData.CampaignCode);
    }

    function ToggleDropdown() {
        IsOpen() ? IsOpen(false) : IsOpen(true);
    }

    function CreateCartItem(baseItem) {
        var item = {};

        _.extend(item, baseItem);

        _.defaults(item, {
            IsInCart: ko.computed(function () {
                var isInCart = _.any(CartItems(), function (cartItem) {
                    return item.Equals(cartItem);
                });

                return isInCart;
            }).extend({ notify: 'always' }),

            AddToCart: function () {
                if (!item.IsInCart()) {
                    // Preliminarily add item to cart.
                    CartItems.push(item);

                    cartApi.AddItem(
                        item,
                        function (result) {
                            _updateCart(result.Cart);
                        },
                        function () {
                            // Failed: remove item
                            CartItems.remove(function (cartItem) {
                                item.Equals(cartItem);
                            });
                        }
                    );
                }
            },

            RemoveFromCart: function () {
                var itemInCart = _.find(CartItems(), function (cartItem) {
                    return item.Equals(cartItem);
                });

                if (itemInCart !== undefined) {
                    // Preliminarily remove item from cart.
                    CartItems.remove(itemInCart);

                    cartApi.RemoveItem(
                        itemInCart,
                        function (result) {
                            _updateCart(result.Cart);
                        },
                        function () {
                            // Failed: add back item.
                            CartItems.push(itemInCart);
                        }
                    );
                }
            },

            ToggleInCart: function () {
                item.IsInCart() ? item.RemoveFromCart() : item.AddToCart();
            },

            Equals: function (other) {
                return item.ArticleNumber === other.ArticleNumber;
            }
        });

        return item;
    }

    function LoadCart(getCartResponse) {
        _updateCart(getCartResponse.data.Cart);
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
        CreateCartItem: CreateCartItem,
        LoadCart: LoadCart
    };
};


if (Atomia.ViewModels.Active !== undefined) {
    Atomia.ViewModels.Active.Cart = Atomia.ViewModels.Cart(_, ko, Atomia.Api.Cart);
}
