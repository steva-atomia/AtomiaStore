/// <reference path="../../../../Scripts/underscore.js" />
/// <reference path="../../../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="atomia.utils.js" />

/* jshint -W079 */
/** @namespace */
var Atomia = Atomia || {};
Atomia.Api = Atomia.Api || {};
Atomia.Api.Cart = Atomia.Api.Cart || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    var onGoingRecalculateRequest = null;

     /**
     * Recalculate cart prices, taxes and totals.
     * @param {Object} cart - The cart to recalculate
     * @param {Function} success - Callback on successful recalculation of cart
     * @param {Function} error - Callback on failed recalculation of cart
     */
    function RecalculateCart(cart, success, error) {

        var request, requestData;

        requestData = {
            CartItems: [],
            CampaignCode: ko.unwrap(cart.CampaignCode)
        };

        _.each(cart.CartItems(), function (item) {
            var cartItem = {
                ArticleNumber: ko.unwrap(item.ArticleNumber),
                RenewalPeriod: ko.unwrap(item.RenewalPeriod),
                Quantity: ko.unwrap(item.Quantity),
                CustomAttributes: ko.unwrap(item.CustomAttributes)
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
