/* jshint -W079 */
var Atomia = Atomia || {};
Atomia._unbound = Atomia._unbound || {};
/* jshint +W079 */

Atomia._unbound.Cart = function (_, ko, amplify) {
    'use strict';

    var CartItems = [],
        SubTotal = 0,
        Total = 0,
        Tax = 0,
        CampaignCode = '';

    function _updateCart(cartData) {
        CartItems = cartData.CartItems;
        SubTotal = cartData.SubTotal;
        Total = cartData.Total;
        Tax = cartData.Tax;
        CampaignCode = cartData.CampaignCode;

        amplify.publish('cart:updated');
    }

    function _addItem(item, success, error) {
        var requestData;

        if (!_.has(item, 'ArticleNumber')) {
            throw new Error('Object must have ArticleNumber property to be added to cart.');
        }

        requestData = _.omit(item, function (value) {
            return _.isFunction(value);
        });

        _.defaults(requestData, {
            RenewalPeriod: {
                Period: 1,
                Unit: 'YEAR'
            },
            Quantity: 1
        });

        amplify.request({
            resourceId: 'Cart.AddItem',
            data: requestData,
            success: function (result) {
                _updateCart(result.Cart);
                item.CartItemId = result.CartItemId;

                if (success !== undefined) {
                    success(result);
                }
            },
            error: function (result) {
                if (error !== undefined) {
                    error(result);
                }
            }
        });
    }

    function _removeItem(item, success, error) {
        var requestData;

        if (!_.has(item, 'CartItemId')) {
            throw new Error('Object must have CartItemId property to be removed from cart.');
        }

        requestData = {
            Id: item.CartItemId
        };

        amplify.request({
            resourceId: 'Cart.RemoveItem',
            data: requestData,
            success: function (result) {
                _updateCart(result);
                delete item.CartItemId;

                if (success !== undefined) {
                    success(result);
                }
            },
            error: function (result) {
                if (error !== undefined) {
                    error(result);
                }
            }
        });
    }

    function _findItem(item) {
        return _.find(CartItems, function (cartItem) {
            return item.equals(cartItem);
        });
    }

    function createCartItemViewModel(options) {
        var item = {},
            itemInCart;

        options = options || {};

        _.defaults(options, {
            init: _.noop,
            equals: function (i1, i2) {
                return i1.ArticleNumber === i2.ArticleNumber;
            }
        });

        options.init(item);

        item.equals = function (itemToCompare) {
            return options.equals(item, itemToCompare);
        };

        itemInCart = _findItem(item);
        if (itemInCart !== undefined) {
            item.CartItemId = itemInCart.Id;
        }

        item.ShouldBeInCart = ko.observable(itemInCart !== undefined);
        item.ShouldBeInCart.subscribe(function (newValue) {
            if (newValue === true && !_findItem(item)) {
                _addItem(item, undefined, function () {
                    item.ShouldBeInCart(false);
                });
            }
            else if (newValue === false && _findItem(item)) {
                _removeItem(item, undefined, function () {
                    item.ShouldBeInCart(true);
                });
            }
        });

        item.addToCart = function () {
            item.ShouldBeInCart(true);
        };

        item.removeFromCart = function () {
            item.ShouldBeInCart(false);
        };

        return item;
    }

    return {
        getCartItems: function () { return CartItems; },
        getSubTotal:  function () { return SubTotal; },
        getTotal:  function () { return Total; },
        getTax:  function () { return Tax; },
        getCampaignCode: function () { return CampaignCode; },
        createCartItemViewModel: createCartItemViewModel
    };
};

Atomia.Cart = Atomia._unbound.Cart(_, ko, amplify);
