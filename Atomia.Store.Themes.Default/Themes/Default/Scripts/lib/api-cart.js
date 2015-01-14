/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.Api = Atomia.Api || {};
Atomia.Api._unbound = Atomia.Api._unbound || {};
/* jshint +W079 */

Atomia.Api._unbound.Cart = function (_, ko, amplify) {
    'use strict';

    function _GetValueOrObservable(value) {
        return _.isFunction(value) ? value() : value;
    }

    function AddItem(item, success, error) {
        var requestData;

        if (!_.has(item, 'ArticleNumber')) {
            throw new Error('Object must have ArticleNumber property to be added to cart.');
        }

        requestData = {
            ArticleNumber: _GetValueOrObservable(item.ArticleNumber),
            RenewalPeriod: _GetValueOrObservable(item.RenewalPeriod),
            Quantity: _GetValueOrObservable(item.Quantity),
            CustomAttributes: _GetValueOrObservable(item.CustomAttributes)
        };

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

    function RemoveItem(item, success, error) {
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
        AddItem: AddItem,
        RemoveItem: RemoveItem
    };
};

Atomia.Api.Cart = Atomia.Api._unbound.Cart(_, ko, amplify);
