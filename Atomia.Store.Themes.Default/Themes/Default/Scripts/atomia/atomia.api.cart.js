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

    var onGoingRecalculateRequest = null;

    function RecalculateCart(cart, success, error) {
        var request, requestData;

        requestData = {
            CartItems: [],
            CampaignCode: _GetValueOrObservable(cart.CampaignCode)
        };

        _.each(cart.CartItems(), function (item) {
            var cartItem = {
                ArticleNumber: _GetValueOrObservable(item.ArticleNumber),
                RenewalPeriod: _GetValueOrObservable(item.RenewalPeriod),
                Quantity: _GetValueOrObservable(item.Quantity),
                CustomAttributes: _GetValueOrObservable(item.CustomAttributes)
            };

            _.defaults(cartItem, {
                RenewalPeriod: {
                    Period: 1,
                    Unit: 'YEAR'
                },
                Quantity: 1
            });

            requestData.CartItems.push(cartItem);
        });

        // Only keep the latest recalculate request open.
        if (onGoingRecalculateRequest !== null) {
            onGoingRecalculateRequest.abort();
        }

        request = utils.request({
            resourceId: 'Cart.RecalculateCart',
            data: requestData,
            success: function (result) {
                onGoingRecalculateRequest = null;

                if (success !== undefined) {
                    success(result);
                }
            },
            error: function (result) {
                onGoingRecalculateRequest = null;

                if (error !== undefined) {
                    error(result);
                }
            }
        });

        onGoingRecalculateRequest = request;
    }


    _.extend(exports, {
        RecalculateCart: RecalculateCart
    });
    
})(Atomia.Api.Cart, _, ko, Atomia.Utils);
