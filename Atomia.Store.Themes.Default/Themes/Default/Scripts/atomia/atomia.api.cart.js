/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.Api = Atomia.Api || {};
Atomia.Api.Cart = Atomia.Api.Cart || {};
/* jshint +W079 */

(function (exports, _, utils) {
    'use strict';

    var onGoingRecalculateRequest = null;

     /**
     * Recalculate cart prices, taxes and totals.
     * @param {Object} cartData - The cart to recalculate
     * @param {Function} success - Callback on successful recalculation of cart
     * @param {Function} error - Callback on failed recalculation of cart
     */
    function recalculateCart(cartData, success, error) {

        var request, requestData;

        requestData = {
            CartItems: cartData.CartItems,
            CampaignCode: cartData.CampaignCode,
            CustomAttributes: cartData.CustomAttributes
        };

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
        recalculateCart: recalculateCart
    });
    
})(Atomia.Api.Cart, _, Atomia.Utils);
