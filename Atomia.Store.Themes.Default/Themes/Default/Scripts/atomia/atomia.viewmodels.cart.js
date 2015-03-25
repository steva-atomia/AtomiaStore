/// <reference path="../../../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../../../Scripts/underscore.js" />
/// <reference path="../../../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="atomia.utils.js" />
/// <reference path="atomia.api.cart.js" />

/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, $, utils, cartApi) {
    'use strict';

    var CartModelPrototype,
        CartItemPrototype,
        CreateCartModel,
        CreateCartItem,
        AddCartItemExtensions;
    
    /* Cart item prototype and factory */
    CartItemPrototype = {

        /** 
         * Equality comparer between this item and 'other'. Defaults to comparing 'Id' properties. 
         * @param {Object} other - The item to compare to.
         */
        Equals: function Equals(other) {
            return this.Id === other.Id;
        },

        /** 
         * Collects custom options (renewal period and domain name) of item.
         * @returns {string[]} Array of custom options.
         */
        CustomOptions: function CustomOptions() {
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

    /**
     * Create a cart item view model.
     * @param {Object|Function} extensions - Extensions to the cart item
     * @param {Object} instance   - The item to create cart item from.
     * @returns The created cart item.
     */
    CreateCartItem = function CreateCartItem(extensions, instance) {
        var defaults = {
                Id: _.uniqueId('tmp-cartitem-')
            };

        return utils.createViewModel(CartItemPrototype, defaults, instance, extensions);
    };

    /**Cart prototype and factory */
    CartModelPrototype = {
        /** Event used to notify Atomia customer validation that cart has been updated. */
        ValidationUpdateEvent: 'cart:update',

        /** Items in cart that are domain items, like registration or transfer */
        DomainItems: function DomainItems() {
            return _.filter(this.CartItems(), function (item) {
                return _.contains(this.DomainCategories, item.Category);
            }.bind(this));
        },

        /** Distinct article numbers of items in cart. */
        ArticleNumbers: function ArticleNumbers() {
            return _.uniq(_.map(this.CartItems(), function (item) {
                return item.ArticleNumber;
            }.bind(this)));
        },

        /** Distinct categories of items in cart. */
        Categories: function Categories() {
            return _.uniq(_.map(this.CartItems(), function (item) {
                return item.Category;
            }.bind(this)));
        },

        /** Open or close cart display. */
        ToggleDropdown: function ToggleDropdown() {
            if (this.IsOpen()) {
                this.IsOpen(false);
            }
            else {
                utils.publish('dropdown:open');
                this.IsOpen(true);
            }
        },

        /** Close cart display */
        CloseDropdown: function CloseDropdown() {
            this.IsOpen(false);
        },

        /** Check if cart contains item that 'Equals' 'item'. */
        Contains: function Contains(item) {
            var isInCart = _.any(this.CartItems(), function (cartItem) {
                return item.Equals(cartItem);
            });

            return isInCart;
        },

        /** Get first item in cart that 'Equals' 'item'. */
        GetExisting: function GetExisting(item) {
            var itemInCart = _.find(this.CartItems(), function (cartItem) {
                return item.Equals(cartItem);
            });

            return itemInCart;
        },

        /** Add 'item' to cart and recalculate. Optionally set 'recalculate' to false. */
        Add: function Add(item, recalculate) {
            var cartItem;

            if (recalculate === undefined) {
                recalculate = true;
            }

            if (!this.Contains(item)) {
                cartItem = this.CreateCartItem(item);
                this.CartItems.push(cartItem);

                if (recalculate === true) {
                    cartApi.RecalculateCart(
                        this,
                        function (result) {
                            this._UpdateCart(result.Cart);

                            utils.publish('cart:add', cartItem);
                        }.bind(this)
                    );
                }
            }
        },

        /** Remove 'item' from cart and recalculate. Optionally set 'recalculate' to false. */
        Remove: function Remove(item, recalculate) {
            var itemInCart;

            if (recalculate === undefined) {
                recalculate = true;
            }

            if (this.Contains(item)) {
                itemInCart = _.find(this.CartItems(), function (cartItem) {
                    return item.Equals(cartItem);
                });

                if (itemInCart !== undefined) {
                    this.CartItems.remove(itemInCart);
                    
                    if (recalculate === true) {
                        cartApi.RecalculateCart(
                            this,
                            function (result) {
                                this._UpdateCart(result.Cart);

                                utils.publish('cart:remove', itemInCart);

                                if (itemInCart.IsDomainItem()) {
                                    this.ClearDomainItem(itemInCart);
                                }
                            }.bind(this)
                        );
                    }
                }
            }
        },

        /** Add 'item' to or remove 'item' from cart and recalculate. Optionally set 'recalculate' to false. */
        AddOrRemove: function AddOrRemove(item, recalculate) {
            this.Contains(item) ? this.Remove(item, recalculate) : this.Add(item, recalculate);
        },

        /** Associate 'domainName' to 'mainItem'. Optionally set 'recalculate' to false. */
        AddDomainName: function AddDomainName(mainItem, domainName, recalculate) {
            var mainInCart = this.GetExisting(mainItem),
                existingDomainName;

            if (recalculate === undefined) {
                recalculate = true;
            }

            if (mainInCart === undefined) {
                throw new Error('mainItem is not in cart.');
            }

            existingDomainName = mainInCart.GetDomainName();

            if (existingDomainName !== domainName) {
                mainInCart.CustomAttributes.push({
                    Name: 'DomainName',
                    Value: domainName
                });

                if (recalculate === true) {
                    cartApi.RecalculateCart(
                        this,
                        function (result) {
                            this._UpdateCart(result.Cart);
                        }.bind(this)
                    );
                }
            }
        },


        /** Remove any domain name associations from 'mainItem'. Optionally set 'recalculate' to false.*/
        RemoveDomainName: function RemoveDomainName(mainItem, recalculate) {
            var mainInCart = this.GetExisting(mainItem);

            if (recalculate === undefined) {
                recalculate = true;
            }

            if (mainInCart === undefined) {
                throw new Error('mainItem is not in cart.');
            }

            if (mainInCart.GetDomainName() !== undefined) {
                mainInCart.CustomAttributes = _.reject(mainInCart.CustomAttributes, function (item) {
                    return item.Name === 'DomainName';
                });

                if (recalculate === true) {
                    cartApi.RecalculateCart(
                        this,
                        function (result) {
                            this._UpdateCart(result.Cart);
                        }.bind(this)
                    );
                }
            }
        },

        /** Remove any associations to domain name tied to 'domainItem' from any other items in cart. */
        ClearDomainItem: function ClearDomainItem(domainItem) {
            var domainNameToRemove = domainItem.GetDomainName();

            _.each(this.CartItems(), function (item) {
                if (!domainItem.Equals(item) && item.GetDomainName() === domainNameToRemove) {
                    this.RemoveDomainName(item);
                }
            }.bind(this));
        },

        /** Load cart with JSON data from 'getCartResponse'. */
        Load: function Load(getCartResponse) {
            this._UpdateCart(getCartResponse.data.Cart);

            if (getCartResponse.data.DomainCategories !== undefined) {
                this.DomainCategories = getCartResponse.data.DomainCategories;
            }
        },

        /** Add 'campaignCode' to cart. Replaces any existing campaign code and recalculates cart. */
        AddCampaignCode: function AddCampaignCode(campaignCode) {
            if (!_.isString(campaignCode) || campaignCode === '') {
                throw new Error('campaignCode must be a non-empty string.');
            }

            this.CampaignCode(campaignCode);

            cartApi.RecalculateCart(
                this,
                function (result) {
                    this._UpdateCart(result.Cart);

                    utils.publish('cart:addCampaignCode', campaignCode);
                }.bind(this)
            );
        },

        /** Remove campaign code from cart and recalculate. */
        RemoveCampaignCode: function RemoveCampaignCode() {
            this.CampaignCode('');

            cartApi.RecalculateCart(
                this,
                function (result) {
                    this._UpdateCart(result.Cart);

                    utils.publish('cart:removeCampaignCode');
                }.bind(this)
            );
        },

        /** Update cart with 'cartData'. */
        _UpdateCart: function _UpdateCart(cartData) {
            this.CartItems.removeAll();

            _.each(cartData.CartItems, function (cartItemData) {
                var item = this.CreateCartItem(cartItemData),
                    cartItem = AddCartItemExtensions(this, item);

                this.CartItems.push(cartItem);
            }.bind(this));

            this.SubTotal(cartData.SubTotal);
            this.Total(cartData.Total);
            this.Tax(cartData.Tax);
            this.CampaignCode(cartData.CampaignCode);

            utils.publish('cart:update');

            // Customer validation plugin expects an event on 
            $('body').trigger(this.ValidationUpdateEvent);
        }
    };

    /**
     * Create view model for cart.
     * @param {Object|Function} extensions       - Extensions to the default cart view model.
     * @param {Object|Function} itemExtensions   - Extensions to the default cart item view model.
     * @returns {Object} The created view model.
     */
    CreateCartModel = function CreateCartModel(extensions, itemExtensions) {
        var defaults, cart;
        
        defaults = function (self) {
            return {
                CreateCartItem: _.partial(CreateCartItem, itemExtensions || {}),
                DomainCategories: [],
                CartItems: ko.observableArray(),
                SubTotal: ko.observable(0),
                Total: ko.observable(0),
                Tax: ko.observable(0),
                CampaignCode: ko.observable(''),
                IsOpen: ko.observable(false),

                NumberOfItems: ko.pureComputed(function NumberOfItems() {
                    return self.CartItems().length;
                }),

                IsEmpty: ko.pureComputed(function IsEmpty() {
                    return self.NumberOfItems() <= 0;
                })
            };
        };

        cart = utils.createViewModel(CartModelPrototype, defaults, extensions);

        utils.subscribe('dropdown:open', function () {
            cart.IsOpen(false);
        });

        return cart;
    };



    /** 
     * Extends 'item' with cart helper methods.
     * @param {Object} cart - The cart to apply helpers to.
     * @param {Object} item - The item to extend with helper methods.
     * @returns {Object} The extended item.
     */
    AddCartItemExtensions = function AddCartItemExtensions(cart, item) {
        var cartExtensions;

        if (item.Equals === undefined) {
            item.Equals = function Equals(other) {
                return item.ArticleNumber === other.ArticleNumber;
            };
        }

        cartExtensions = {
            IsInCart: ko.computed(function IsInCart() {
                return cart.Contains(item);
            }).extend({ notify: 'always' }),
            
            GetItemInCart: function GetItemInCart() {
                return cart.GetExisting(item);
            },

            AddToCart: function AddToCart() {
                return cart.Add(item);
            },

            RemoveFromCart: function RemoveFromCart() {
                return cart.Remove(item);
            },

            ToggleInCart: function ToggleInCart() {
                cart.AddOrRemove(item);
            },

            GetDomainName: function GetDomainName() {
                var domainAttr, domainName;

                if (this.CustomAttributes !== undefined) {
                    domainAttr = _.findWhere(item.CustomAttributes, { Name: 'DomainName' });
                }

                if (domainAttr !== undefined) {
                    domainName = domainAttr.Value;
                }

                return domainName;
            },

            IsDomainItem: function IsDomainItem() {
                return _.contains(cart.DomainCategories, item.Category);
            },

            IsRemovable: function IsRemovable() {
                var notRemovable = _.any(item.CustomAttributes, function (ca) {
                    return ca.Name === 'NotRemovable' && ca.Value !== 'false';
                });

                return !notRemovable;
            }
        };

        return _.extend(item, cartExtensions);
    };

    

    /* Module exports */
    _.extend(exports, {
        CreateCartModel: CreateCartModel,
        AddCartItemExtensions: AddCartItemExtensions
    });

})(Atomia.ViewModels, _, ko, jQuery, Atomia.Utils, Atomia.Api.Cart);
