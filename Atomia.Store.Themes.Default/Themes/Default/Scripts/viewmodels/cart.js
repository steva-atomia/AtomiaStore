/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (module, _, ko, amplify, cartApi) {
    'use strict';

    var ItemInCart, Cart;

    /* ItemInCart and prototype */
    ItemInCart = function ItemInCart(itemData, tmpId) {
        _.extend(this, itemData);

        if (tmpId !== undefined) {
            this.Id = tmpId;
        }

        _.bindAll(this, 'Equals', 'CustomOptions');
    };

    ItemInCart.prototype = {
        Equals: function (other) {
            return this.Id === other.Id;
        },
        CustomOptions: function () {
            var options = [],
                domainName;

            if (this.RenewalPeriod) {
                options.push(this.RenewalPeriod.Display);
            }

            if (!this.IsDomainItem()) {
                domainName = this.GetDomainName();

                if (domainName !== undefined) {
                    options.push(domainName);
                }
            }

            return options;
        }
    };

    /* Cart and prototype */
    Cart = function Cart() {
        this.ItemInCart = ItemInCart;

        this.DomainCategories = ['Domain'];

        this.CartItems = ko.observableArray();
        this.SubTotal = ko.observable(0);
        this.Total = ko.observable(0);
        this.Tax = ko.observable(0);
        this.CampaignCode = ko.observable('');
        this.IsOpen = ko.observable(false);

        this.NumberOfItems = ko.pureComputed(this.NumberOfItems, this);
        this.IsEmpty = ko.pureComputed(this.IsEmpty, this);

        _.bindAll(this, '_UpdateCart', 'ToggleDropdown', 'ExtendWithCartProperties', 'Load',
            'Contains', 'GetExisting', 'Add', 'Remove', 'AddOrRemove', 'DomainItems',
            'AddDomainName', 'RemoveDomainName', 'ClearDomainItem');
    };

    Cart.prototype = {
        _UpdateCart: function (cartData) {
            this.CartItems.removeAll();

            _.each(cartData.CartItems, function (cartItemData) {
                var item = new this.ItemInCart(cartItemData),
                    cartItem = this.ExtendWithCartProperties(item);

                this.CartItems.push(cartItem);
            }.bind(this));

            this.SubTotal(cartData.SubTotal);
            this.Total(cartData.Total);
            this.Tax(cartData.Tax);
            this.CampaignCode(cartData.CampaignCode);

            amplify.publish('cart:update');
        },

        DomainItems: function() {
            return _.filter(this.CartItems(), function (item) {
                return _.contains(this.DomainCategories, item.Category);
            }.bind(this));
        },

        NumberOfItems: function () {
            return this.CartItems().length;
        },

        IsEmpty: function () {
            return this.NumberOfItems() <= 0;
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
            var self = this,
                cartItem;

            if (!this.Contains(item)) {
                cartItem = new ItemInCart(item, _.uniqueId('tmp-cartitem-'));

                // Preliminarily add item to cart.
                this.CartItems.push(cartItem);

                cartApi.AddItem(
                    cartItem,
                    function (result) {
                        self._UpdateCart(result.Cart);

                        amplify.publish('cart:add', cartItem);
                    },
                    function () {
                        // Failed: remove item
                        self.CartItems.remove(function (itemToRemove) {
                            cartItem.Equals(itemToRemove);
                        });
                    }
                );
            }
        },

        Remove: function(item) {
            var itemInCart;

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
                            this._UpdateCart(result.Cart);

                            amplify.publish('cart:remove', itemInCart);

                            if (itemInCart.IsDomainItem()) {
                                this.ClearDomainItem(itemInCart);
                            }
                        }.bind(this),
                        function () {
                            // Failed: add back item.
                            this.CartItems.push(itemInCart);
                        }.bind(this)
                    );
                }
            }
        },

        Update: function(item) {
            this.Remove(item);
            this.Add(item);
        },

        AddOrRemove: function(item) {
            this.Contains(item) ? this.Remove(item) : this.Add(item);
        },

        AddDomainName: function (mainItem, domainName) {
            var mainInCart = this.GetExisting(mainItem),
                existingDomainName;

            if (mainInCart === undefined) {
                throw new Error('mainItem is not in cart.');
            }

            existingDomainName = mainInCart.GetDomainName();

            if (existingDomainName !== domainName) {
                cartApi.SetItemAttribute(
                    mainInCart, 'DomainName', domainName,
                    function (result) {
                        this._UpdateCart(result.Cart);
                    }.bind(this)
                );
            }
        },

        RemoveDomainName: function(mainItem) {
            var mainInCart = this.GetExisting(mainItem);

            if (mainInCart === undefined) {
                throw new Error('mainItem is not in cart.');
            }

            if (mainInCart.GetDomainName() !== undefined) {
                cartApi.RemoveItemAttribute(
                    mainInCart, 'DomainName',
                    function (result) {
                        this._UpdateCart(result.Cart);
                    }.bind(this)
                );
            }
        },

        ClearDomainItem: function (domainItem) {
            var domainNameToRemove = domainItem.GetDomainName();

            _.each(this.CartItems(), function (item) {
                if (!domainItem.Equals(item) && item.GetDomainName() === domainNameToRemove) {
                    this.RemoveDomainName(item);
                }
            }.bind(this));
        },

        ExtendWithCartProperties: function (item) {
            var cart = this;
            
            if (item.Equals === undefined) {
                item.Equals = function (other) {
                    return item.ArticleNumber === other.ArticleNumber;
                };
            }

            // Add convenience properties to items for easier data binding
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
                },

                GetDomainName: function() {
                    var domainAttr, domainName;

                    if (item.CustomAttributes !== undefined) {
                        domainAttr = _.findWhere(item.CustomAttributes, { Name: 'DomainName' });
                    }

                    if (domainAttr !== undefined) {
                        domainName = domainAttr.Value;
                    }
                    
                    return domainName;
                },

                IsDomainItem: function () {
                    return _.contains(cart.DomainCategories, item.Category);
                }
            });

            return item;
        },

        Load: function (getCartResponse) {
            this._UpdateCart(getCartResponse.data.Cart);

            if (getCartResponse.data.DomainCategories !== undefined) {
                this.DomainCategories = getCartResponse.data.DomainCategories;
            }
        }
    };



    /* Export models */
    module.ItemInCart = ItemInCart;
    module.Cart = Cart;

})(Atomia.ViewModels, _, ko, amplify, Atomia.Api.Cart);
