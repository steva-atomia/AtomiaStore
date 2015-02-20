/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, $, utils, cartApi) {
    'use strict';

    var CartPrototype,
        CartItemPrototype,
        CreateCart,
        CreateCartItem,
        AddCartExtensions;


    /* Cart item prototype and factory */
    CartItemPrototype = {
        Equals: function Equals(other) {
            return this.Id === other.Id;
        },
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

    CreateCartItem = function CreateCartItem(extensions, instance) {
        var defaults = {
                Id: _.uniqueId('tmp-cartitem-')
            };

        return utils.createViewModel(CartItemPrototype, defaults, instance, extensions);
    };


    /* Cart prototype and factory */
    CartPrototype = {
        ValidationUpdateEvent: 'cart:update',

        DomainItems: function DomainItems() {
            return _.filter(this.CartItems(), function (item) {
                return _.contains(this.DomainCategories, item.Category);
            }.bind(this));
        },

        ArticleNumbers: function ArticleNumbers() {
            return _.uniq(_.map(this.CartItems(), function (item) {
                return item.ArticleNumber;
            }.bind(this)));
        },

        Categories: function Categories() {
            return _.uniq(_.map(this.CartItems(), function (item) {
                return item.Category;
            }.bind(this)));
        },

        ToggleDropdown: function ToggleDropdown() {
            this.IsOpen() ? this.IsOpen(false) : this.IsOpen(true);
        },

        Contains: function Contains(item) {
            var isInCart = _.any(this.CartItems(), function (cartItem) {
                return item.Equals(cartItem);
            });

            return isInCart;
        },

        GetExisting: function GetExisting(item) {
            var itemInCart = _.find(this.CartItems(), function (cartItem) {
                return item.Equals(cartItem);
            });

            return itemInCart;
        },

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

        AddOrRemove: function AddOrRemove(item, recalculate) {
            this.Contains(item) ? this.Remove(item, recalculate) : this.Add(item, recalculate);
        },

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

        ClearDomainItem: function ClearDomainItem(domainItem) {
            var domainNameToRemove = domainItem.GetDomainName();

            _.each(this.CartItems(), function (item) {
                if (!domainItem.Equals(item) && item.GetDomainName() === domainNameToRemove) {
                    this.RemoveDomainName(item);
                }
            }.bind(this));
        },

        Load: function Load(getCartResponse) {
            this._UpdateCart(getCartResponse.data.Cart);

            if (getCartResponse.data.DomainCategories !== undefined) {
                this.DomainCategories = getCartResponse.data.DomainCategories;
            }
        },

        _UpdateCart: function _UpdateCart(cartData) {
            this.CartItems.removeAll();

            _.each(cartData.CartItems, function (cartItemData) {
                var item = this.CreateCartItem(cartItemData),
                    cartItem = AddCartExtensions(this, item);

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

    CreateCart = function CreateCart(extensions, itemExtensions) {
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

        cart = utils.createViewModel(CartPrototype, defaults, extensions);

        return cart;
    };



    /* AddCartExtensions item extender */
    AddCartExtensions = function AddCartExtensions(cart, item) {
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
        CreateCart: CreateCart,
        AddCartExtensions: AddCartExtensions
    });

})(Atomia.ViewModels, _, ko, jQuery, Atomia.Utils, Atomia.Api.Cart);
