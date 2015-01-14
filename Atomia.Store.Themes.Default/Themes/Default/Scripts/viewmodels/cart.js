/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (module, _, ko, cartApi) {
    'use strict';

    var ItemInCart, Cart;

    ItemInCart = function ItemInCart(itemData) {
        _.extend(this, itemData);

        this.CartItemId = this.Id;

        _.bindAll(this, 'Equals');
    };

    ItemInCart.prototype = {
        Equals: function (other) {
            return this.CartItemId === other.CartItemId;
        }
    };

    Cart = function Cart() {
        this.ItemInCart = ItemInCart;

        this.CartItems = ko.observableArray();
        this.SubTotal = ko.observable(0);
        this.Total = ko.observable(0);
        this.Tax = ko.observable(0);
        this.CampaignCode = ko.observable('');
        this.IsOpen = ko.observable(false);

        this.NumberOfItems = ko.pureComputed(this.NumberOfItems, this);
        this.IsEmpty = ko.pureComputed(this.IsEmpty, this);

        _.bindAll(this, '_UpdateCart', 'ToggleDropdown', 'MakeCartItem', 'Load');
    };

    Cart.prototype = {
        NumberOfItems: function () {
            return this.CartItems().length;
        },

        IsEmpty: function () {
            return this.NumberOfItems() <= 0;
        },

        _UpdateCart: function (cartData) {
            var self = this;

            this.CartItems.removeAll();

            _.each(cartData.CartItems, function (cartItemData) {
                var item = new self.ItemInCart(cartItemData),
                    cartItem = self.MakeCartItem(item);

                self.CartItems.push(cartItem);
            });

            this.SubTotal(cartData.SubTotal);
            this.Total(cartData.Total);
            this.Tax(cartData.Tax);
            this.CampaignCode(cartData.CampaignCode);
        },

        ToggleDropdown: function () {
            this.IsOpen() ? this.IsOpen(false) : this.IsOpen(true);
        },

        MakeCartItem: function (item) {
            var cart = this;
            
            // Must be defined before IsInCart.
            if (item.Equals === undefined) {
                item.Equals = function (other) {
                    return item.ArticleNumber === other.ArticleNumber;
                };
            }

            _.defaults(item, {
                IsInCart: ko.computed(function () {
                    var isInCart = _.any(cart.CartItems(), function (cartItem) {
                        return item.Equals(cartItem);
                    });

                    return isInCart;
                }).extend({ notify: 'always' }),

                GetItemInCart: function () {
                    var itemInCart = _.find(cart.CartItems(), function (cartItem) {
                        return item.Equals(cartItem);
                    });

                    return itemInCart;
                },

                AddToCart: function () {
                    if (!item.IsInCart()) {
                        // Preliminarily add item to cart.
                        cart.CartItems.push(item);

                        cartApi.AddItem(
                            item,
                            function (result) {
                                cart._UpdateCart(result.Cart);
                            },
                            function () {
                                // Failed: remove item
                                cart.CartItems.remove(function (cartItem) {
                                    item.Equals(cartItem);
                                });
                            }
                        );
                    }
                },

                RemoveFromCart: function () {
                    var itemInCart;

                    if (item.IsInCart()) {
                        itemInCart = _.find(cart.CartItems(), function (cartItem) {
                            return item.Equals(cartItem);
                        });

                        if (itemInCart !== undefined) {
                            // Preliminarily remove item from cart.
                            cart.CartItems.remove(itemInCart);

                            cartApi.RemoveItem(
                                itemInCart,
                                function (result) {
                                    cart._UpdateCart(result.Cart);
                                },
                                function () {
                                    // Failed: add back item.
                                    cart.CartItems.push(itemInCart);
                                }
                            );
                        }
                    }
                },

                ToggleInCart: function () {
                    item.IsInCart() ? item.RemoveFromCart() : item.AddToCart();
                }
            });

            return item;
        },

        Load: function (getCartResponse) {
            this._UpdateCart(getCartResponse.data.Cart);
        }
    };

    module.ItemInCart = ItemInCart;
    module.Cart = Cart;

})(Atomia.ViewModels, _, ko, Atomia.Api.Cart);
