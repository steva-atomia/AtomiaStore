/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (module, _, ko, cartApi) {
    'use strict';

    var ItemInCart, Cart;

    /* ItemInCart and prototype */
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



    /* Cart and prototype */
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

        _.bindAll(this, '_UpdateCart', 'ToggleDropdown', 'ExtendWithCartProperties', 'Load',
            'Contains', 'GetExisting', 'Add', 'Remove', 'AddOrRemove');
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
                    cartItem = self.ExtendWithCartProperties(item);

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

        Contains: function(item) {
            var isInCart = _.any(this.CartItems(), function (cartItem) {
                return item.Equals(cartItem);
            });

            return isInCart;
        },

        GetExisting: function(item) {
            var itemInCart = _.find(this.CartItems(), function (cartItem) {
                return item.Equals(cartItem);
            });

            return itemInCart;
        },

        Add: function (item) {
            var self = this;

            if (!this.Contains(item)) {
                // Preliminarily add item to cart.
                this.CartItems.push(item);

                cartApi.AddItem(
                    item,
                    function (result) {
                        self._UpdateCart(result.Cart);
                    },
                    function () {
                        // Failed: remove item
                        self.CartItems.remove(function (cartItem) {
                            item.Equals(cartItem);
                        });
                    }
                );
            }
        },

        Remove: function(item) {
            var itemInCart,
                self = this;

            if (this.Contains(item)) {
                itemInCart = _.find(this.CartItems(), function (cartItem) {
                    return item.Equals(cartItem);
                });

                if (itemInCart !== undefined) {
                    // Preliminarily remove item from cart.
                    this.CartItems.remove(itemInCart);

                    cartApi.RemoveItem(
                        itemInCart,
                        function (result) {
                            self._UpdateCart(result.Cart);
                        },
                        function () {
                            // Failed: add back item.
                            self.CartItems.push(itemInCart);
                        }
                    );
                }
            }
        },

        AddOrRemove: function(item) {
            this.Contains(item) ? this.Remove(item) : this.Add(item)
        },

        ExtendWithCartProperties: function (item) {
            var cart = this;
            
            if (item.Equals === undefined) {
                item.Equals = function (other) {
                    return item.ArticleNumber === other.ArticleNumber;
                };
            }

            _.defaults(item, {
                IsInCart: ko.computed(function () {
                    return cart.Contains(item);
                }).extend({ notify: 'always' }),

                GetItemInCart: function () {
                    return cart.GetExisting(item);
                },

                AddToCart: function () {
                    return cart.Add(item);
                },

                RemoveFromCart: function () {
                    return cart.Remove(item);
                },

                ToggleInCart: function () {
                    cart.AddOrRemove(item);
                }
            });

            return item;
        },

        Load: function (getCartResponse) {
            this._UpdateCart(getCartResponse.data.Cart);
        }
    };



    /* Export models */
    module.ItemInCart = ItemInCart;
    module.Cart = Cart;

})(Atomia.ViewModels, _, ko, Atomia.Api.Cart);
