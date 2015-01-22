/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.Api = Atomia.Api || {};
Atomia.Api.Cart = Atomia.Api.Cart || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
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

        utils.request({
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

        if (!_.has(item, 'Id')) {
            throw new Error('Object must have Id property to be removed from cart.');
        }

        requestData = {
            Id: item.Id
        };

        utils.request({
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

    function SetItemAttribute(item, attributeName, attributeValue, success, error) {
        var requestData;

        if (!_.has(item, 'Id')) {
            throw new Error('Object must have Id property to have attribute set.');
        }

        if (attributeName === undefined) {
            throw new Error('Missing argument attributeName.');
        }

        if (attributeValue === undefined) {
            throw new Error('Missing argument attributeValue.');
        }

        requestData = {
            Id: item.Id,
            AttributeName: attributeName,
            AttributeValue: attributeValue
        };

        utils.request({
            resourceId: 'Cart.SetItemAttribute',
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

    function RemoveItemAttribute(item, attributeName, success, error) {
        var requestData;

        if (!_.has(item, 'Id')) {
            throw new Error('Object must have Id property to have attribute set.');
        }

        if (attributeName === undefined) {
            throw new Error('Missing argument attributeName.');
        }

        requestData = {
            Id: item.Id,
            AttributeName: attributeName
        };

        utils.request({
            resourceId: 'Cart.RemoveItemAttribute',
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


    _.extend(exports, {
        AddItem: AddItem,
        RemoveItem: RemoveItem,
        SetItemAttribute: SetItemAttribute,
        RemoveItemAttribute: RemoveItemAttribute
    });
    
})(Atomia.Api.Cart, _, ko, Atomia.Utils);
