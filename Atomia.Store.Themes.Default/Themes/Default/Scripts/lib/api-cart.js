/* jshint -W079 */
var Atomia = Atomia || {};
Atomia._unbound = Atomia._unbound || {};
/* jshint +W079 */

Atomia._unbound.Cart = function (_, request) {
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
    }

    function addItem(item, success, error) {
        var requestData;

        if (!_.has(item, 'ArticleNumber')) {
            throw new Error('Object must have ArticleNumber property to be added to cart.');
        }

        requestData = _.pick(item, 'ArticleNumber', 'RenewalPeriod', 'Quantity');
        _.defaults(requestData, {
            RenewalPeriod: {
                Period: 1,
                Unit: 'YEAR'
            },
            Quantity: 1
        });

        request({
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

    function removeItem(item, success, error) {
        var requestData;

        if (!_.has(item, 'CartItemId')) {
            throw new Error('Object must have CartItemId property to be removed from cart.');
        }

        requestData = {
            Id: item.CartItemId
        };

        request({
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

    return {
        getCartItems: function () { return CartItems; },
        getSubTotal:  function () { return SubTotal; },
        getTotal:  function () { return Total; },
        getTax:  function () { return Tax; },
        getCampaignCode: function () { return CampaignCode; },
        addItem: addItem,
        removeItem: removeItem
    };
};

Atomia.Cart = Atomia._unbound.Cart(_, amplify.request);
