/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, $, utils, cartApi) {
    'use strict';

    /**
     * Create a cart item view model.
     * @param {Object} instance - The item to create cart item from.
     * @returns The created cart item.
     */
    function CartItem(cartItemData) {
        var self = this;

        self.id = _.uniqueId('tmp-cartitem-');
        
        /** 
         * Equality comparer between this item and 'other'. Defaults to comparing 'Id' properties. 
         * @param {Object} other - The item to compare to.
         */
        self.equals = function equals(other) {
            return self.id === other.id;
        };

        /** 
         * Collects custom options (renewal period and domain name) of item.
         * @returns {string[]} Array of custom options.
         */
        self.customOptions = function customOptions() {
            var options = [],
                domainName;

            if (self.RenewalPeriod) {
                options.push(self.RenewalPeriod.Display);
            }

            if (!self.isDomainItem()) {
                domainName = self.getDomainName();

                if (domainName !== undefined) {
                    options.push(domainName);
                }
            }

            return options;
        };

        _.extend(self, cartItemData);
    };

    
    /** Create view model for cart. */
    function CartModel() {
        var self = this;
        
        self.DomainCategories = [];
        self.CartItems = ko.observableArray();
        self.SubTotal = ko.observable(0);
        self.Total = ko.observable(0);
        self.Tax = ko.observable(0);
        self.CampaignCode = ko.observable('');
        self.IsOpen = ko.observable(false);
        
        self.NumberOfItems = ko.pureComputed(function NumberOfItems() {
            return self.CartItems().length;
        });
        
        self.IsEmpty = ko.pureComputed(function IsEmpty() {
            return self.NumberOfItems() <= 0;
        });

        self.CreateCartItem = function(cartItemData) {
            return new CartItem(cartItemData);
        };

        /** Event used to notify Atomia customer validation that cart has been updated. */
        self.ValidationUpdateEvent = 'cart:update';

        /** Items in cart that are domain items, like registration or transfer */
        self.DomainItems = function DomainItems() {
            return _.filter(self.CartItems(), function (item) {
                return _.contains(self.DomainCategories, item.Category);
            });
        };

        /** Distinct article numbers of items in cart. */
        self.ArticleNumbers = function ArticleNumbers() {
            return _.uniq(_.map(self.CartItems(), function (item) {
                return item.ArticleNumber;
            }));
        };

        /** Distinct categories of items in cart. */
        self.Categories = function Categories() {
            return _.uniq(_.map(self.CartItems(), function (item) {
                return item.Category;
            }));
        };

        /** Open or close cart display. */
        self.ToggleDropdown = function ToggleDropdown() {
            if (self.IsOpen()) {
                self.IsOpen(false);
            }
            else {
                utils.publish('dropdown:open');
                self.IsOpen(true);
            }
        };

        /** Close cart display */
        self.CloseDropdown = function CloseDropdown() {
            self.IsOpen(false);
        };

        /** Check if cart contains item that 'equals' 'item'. */
        self.Contains = function Contains(item) {
            var isInCart = _.any(self.CartItems(), function (cartItem) {
                return item.equals(cartItem);
            });

            return isInCart;
        };

        /** Get first item in cart that 'equals' 'item'. */
        self.GetExisting = function GetExisting(item) {
            var itemInCart = _.find(self.CartItems(), function (cartItem) {
                return item.equals(cartItem);
            });

            return itemInCart;
        };

        /** Add 'item' to cart and recalculate. Optionally set 'recalculate' to false. */
        self.Add = function Add(item, recalculate) {
            var cartItem;

            if (recalculate === undefined) {
                recalculate = true;
            }

            if (!self.Contains(item)) {
                cartItem = self.CreateCartItem(item);
                self.CartItems.push(cartItem);

                if (recalculate === true) {
                    cartApi.RecalculateCart(self, function (result) {
                        self._UpdateCart(result.Cart);

                        utils.publish('cart:add', cartItem);
                    });
                }
            }
        };

        /** Remove 'item' from cart and recalculate. Optionally set 'recalculate' to false. */
        self.Remove = function Remove(item, recalculate) {
            var itemInCart;

            if (recalculate === undefined) {
                recalculate = true;
            }

            if (self.Contains(item)) {
                itemInCart = _.find(self.CartItems(), function (cartItem) {
                    return item.equals(cartItem);
                });

                if (itemInCart !== undefined) {
                    self.CartItems.remove(itemInCart);

                    if (recalculate === true) {
                        cartApi.RecalculateCart(self, function (result) {
                            self._UpdateCart(result.Cart);

                            utils.publish('cart:remove', itemInCart);

                            if (itemInCart.isDomainItem()) {
                                self.ClearDomainItem(itemInCart);
                            }
                        });
                    }
                }
            }
        };

        /** Add 'item' to or remove 'item' from cart and recalculate. Optionally set 'recalculate' to false. */
        self.AddOrRemove = function AddOrRemove(item, recalculate) {
            self.Contains(item) ? self.Remove(item, recalculate) : self.Add(item, recalculate);
        };

        /** Associate 'domainName' to 'mainItem'. Optionally set 'recalculate' to false. */
        self.AddDomainName = function AddDomainName(mainItem, domainName, recalculate) {
            var mainInCart = self.GetExisting(mainItem),
                existingDomainName;

            if (recalculate === undefined) {
                recalculate = true;
            }

            if (mainInCart === undefined) {
                throw new Error('mainItem is not in cart.');
            }

            existingDomainName = mainInCart.getDomainName();

            if (existingDomainName !== domainName) {
                mainInCart.CustomAttributes.push({
                    Name: 'DomainName',
                    Value: domainName
                });

                if (recalculate === true) {
                    cartApi.RecalculateCart(self, function (result) {
                        self._UpdateCart(result.Cart);
                    });
                }
            }
        };

        /** Remove any domain name associations from 'mainItem'. Optionally set 'recalculate' to false.*/
        self.RemoveDomainName = function RemoveDomainName(mainItem, recalculate) {
            var mainInCart = self.GetExisting(mainItem);

            if (recalculate === undefined) {
                recalculate = true;
            }

            if (mainInCart === undefined) {
                throw new Error('mainItem is not in cart.');
            }

            if (mainInCart.getDomainName() !== undefined) {
                mainInCart.CustomAttributes = _.reject(mainInCart.CustomAttributes, function (item) {
                    return item.Name === 'DomainName';
                });

                if (recalculate === true) {
                    cartApi.RecalculateCart(self, function (result) {
                        self._UpdateCart(result.Cart);
                    });
                }
            }
        };

        /** Remove any associations to domain name tied to 'domainItem' from any other items in cart. */
        self.ClearDomainItem = function ClearDomainItem(domainItem) {
            var domainNameToRemove = domainItem.getDomainName();

            _.each(self.CartItems(), function (item) {
                if (!domainItem.equals(item) && item.getDomainName() === domainNameToRemove) {
                    self.RemoveDomainName(item);
                }
            });
        };

        /** Load cart with JSON data from 'getCartResponse'. */
        self.Load = function Load(getCartResponse) {
            self._UpdateCart(getCartResponse.data.Cart);

            if (getCartResponse.data.DomainCategories !== undefined) {
                self.DomainCategories = getCartResponse.data.DomainCategories;
            }
        };

        /** Add 'campaignCode' to cart. Replaces any existing campaign code and recalculates cart. */
        self.AddCampaignCode = function AddCampaignCode(campaignCode) {
            if (!_.isString(campaignCode) || campaignCode === '') {
                throw new Error('campaignCode must be a non-empty string.');
            }

            self.CampaignCode(campaignCode);

            cartApi.RecalculateCart(self, function (result) {
                self._UpdateCart(result.Cart);

                utils.publish('cart:addCampaignCode', campaignCode);
            });
        };

        /** Remove campaign code from cart and recalculate. */
        self.RemoveCampaignCode = function RemoveCampaignCode() {
            self.CampaignCode('');

            cartApi.RecalculateCart(self, function (result) {
                self._UpdateCart(result.Cart);

                utils.publish('cart:removeCampaignCode');
            });
        };

        /** Update cart with 'cartData'. */
        self._UpdateCart = function _UpdateCart(cartData) {
            self.CartItems.removeAll();

            _.each(cartData.CartItems, function (cartItemData) {
                var item = self.CreateCartItem(cartItemData);
                var cartItem = AddCartItemExtensions(self, item);

                self.CartItems.push(cartItem);
            });

            self.SubTotal(cartData.SubTotal);
            self.Total(cartData.Total);
            self.Tax(cartData.Tax);
            self.CampaignCode(cartData.CampaignCode);

            utils.publish('cart:update');

            // Customer validation plugin expects an event on 
            $('body').trigger(self.ValidationUpdateEvent);
        };

        
        utils.subscribe('dropdown:open', function () {
            self.IsOpen(false);
        });
    }



    /** 
     * Extends 'item' with cart helper methods.
     * @param {Object} cart - The cart to apply helpers to.
     * @param {Object} item - The item to extend with helper methods.
     * @returns {Object} The extended item.
     */
    function AddCartItemExtensions(cart, item) {
        var cartExtensions;

        if (item.equals === undefined) {
            item.equals = function equals(other) {
                return item.ArticleNumber === other.ArticleNumber;
            };
        }

        cartExtensions = {
            isInCart: ko.computed(function isInCart() {
                return cart.Contains(item);
            }).extend({ notify: 'always' }),
            
            getItemInCart: function getItemInCart() {
                return cart.GetExisting(item);
            },

            addToCart: function addToCart() {
                return cart.Add(item);
            },

            removeFromCart: function removeFromCart() {
                return cart.Remove(item);
            },

            toggleInCart: function toggleInCart() {
                cart.AddOrRemove(item);
            },

            getDomainName: function getDomainName() {
                var domainAttr, domainName;

                if (item.CustomAttributes !== undefined) {
                    domainAttr = _.findWhere(item.CustomAttributes, { Name: 'DomainName' });
                }

                if (domainAttr !== undefined) {
                    domainName = domainAttr.Value;
                }

                return domainName;
            },

            isDomainItem: function isDomainItem() {
                return _.contains(cart.DomainCategories, item.Category);
            },

            isRemovable: function isRemovable() {
                var notRemovable = _.any(item.CustomAttributes, function (ca) {
                    return ca.Name === 'NotRemovable' && ca.Value !== 'false';
                });

                return !notRemovable;
            }
        };

        return _.extend(item, cartExtensions);
    }

    

    /* Module exports */
    _.extend(exports, {
        CartModel: CartModel,
        AddCartItemExtensions: AddCartItemExtensions
    });

})(Atomia.ViewModels, _, ko, jQuery, Atomia.Utils, Atomia.Api.Cart);
