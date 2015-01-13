/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */


(function (module, _) {
    'use strict';

    var ItemInCart = function ItemInCart(itemData) {
        _.extend(this, itemData);

        this.CartItemId = this.Id;
        this.Equals = this._Equals.bind(this);
    };

    ItemInCart.prototype = {
        _Equals: function (other) {
            return this.CartItemId === other.CartItemId;
        }
    };
    
    module.ItemInCart = ItemInCart;

})(Atomia.ViewModels, _);


(function (module, _, ko, cartApi) {
    'use strict';

    var Cart = function Cart(ItemInCart) {

        this._ItemInCart = ItemInCart;

        this.CartItems = ko.observableArray();
        this.SubTotal = ko.observable(0);
        this.Total = ko.observable(0);
        this.Tax = ko.observable(0);
        this.CampaignCode = ko.observable('');
        this.IsOpen = ko.observable(false);

        this.NumberOfItems = ko.pureComputed(this._NumberOfItems, this);
        this.IsEmpty = ko.pureComputed(this._IsEmpty, this);

        this.UpdateCart = this._UpdateCart.bind(this);
        this.ToggleDropdown = this._ToggleDropdown.bind(this);
        this.MakeCartItem = this._MakeCartItem.bind(this);
        this.Load = this._Load.bind(this);
    };

    Cart.prototype = {
        _NumberOfItems: function () {
            return this.CartItems().length;
        },
        _IsEmpty: function () {
            return this.NumberOfItems() <= 0;
        },
        _UpdateCart: function (cartData) {
            var self = this;

            this.CartItems.removeAll();

            _.each(cartData.CartItems, function (cartItemData) {
                var item = new self._ItemInCart(cartItemData),
                    cartItem = self.MakeCartItem(item);

                self.CartItems.push(cartItem);
            });

            this.SubTotal(cartData.SubTotal);
            this.Total(cartData.Total);
            this.Tax(cartData.Tax);
            this.CampaignCode(cartData.CampaignCode);
        },

        _ToggleDropdown: function () {
            this.IsOpen() ? this.IsOpen(false) : this.IsOpen(true);
        },

        _MakeCartItem: function (item) {
            var cart = this;

            _.defaults(item, {
                IsInCart: ko.computed(function () {
                    var isInCart = _.any(cart.CartItems(), function (cartItem) {
                        return item.Equals(cartItem);
                    });

                    return isInCart;
                }).extend({ notify: 'always' }),

                AddToCart: function () {
                    if (!item.IsInCart()) {
                        // Preliminarily add item to cart.
                        cart.CartItems.push(item);

                        cartApi.AddItem(
                            item,
                            function (result) {
                                cart.UpdateCart(result.Cart);
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
                    var itemInCart = _.find(cart.CartItems(), function (cartItem) {
                        return item.Equals(cartItem);
                    });

                    if (itemInCart !== undefined) {
                        // Preliminarily remove item from cart.
                        cart.CartItems.remove(itemInCart);

                        cartApi.RemoveItem(
                            itemInCart,
                            function (result) {
                                cart.UpdateCart(result.Cart);
                            },
                            function () {
                                // Failed: add back item.
                                cart.CartItems.push(itemInCart);
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
        },

        _Load: function (getCartResponse) {
            this.UpdateCart(getCartResponse.data.Cart);
        }
    };

    module.Cart = Cart;

})(Atomia.ViewModels, _, ko, Atomia.Api.Cart);
