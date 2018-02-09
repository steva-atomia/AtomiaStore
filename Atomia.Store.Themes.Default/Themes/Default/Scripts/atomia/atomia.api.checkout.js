/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.Api = Atomia.Api || {};
Atomia.Api.Checkout = Atomia.Api.Checkout || {};
/* jshint +W079 */

(function (exports, _, utils) {
    'use strict';

    var onGoingValidationRequest = null;
    var onGoingTermsOfServiceRequest = null;

    /**
    * Validate VAT number
    */
    function validateVatNumber(vatNumber, success, error) {

        var request, requestData;

        if (vatNumber !== '' && vatNumber != null) {
            requestData = {
                vatNumber: vatNumber
            };
        }

        // Only keep the latest validation request open.
        if (onGoingValidationRequest !== null) {
            onGoingValidationRequest.abort();
        }

        request = utils.request({
            resourceId: 'Checkout.ValidateVatNumber',
            data: requestData,
            success: function (result) {
                onGoingValidationRequest = null;

                if (success !== undefined) {
                    success(result);
                }
            },
            error: function (result) {
                onGoingValidationRequest = null;

                if (error !== undefined) {
                    error(result);
                }
            }
        });

        onGoingValidationRequest = request;
    }

    /**
    * Get Terms Of Service
    */
    function getTermsOfService(success, error) {
        var request, requestData;

        requestData = {};

        if (onGoingTermsOfServiceRequest !== null) {
            onGoingTermsOfServiceRequest.abort();
        }

        request = utils.request({
            resourceId: 'Checkout.TermsOfService',
            data: requestData,
            success: function (result) {
                onGoingTermsOfServiceRequest = null;

                if (success !== undefined) {
                    success(result);
                }
            },
            error: function (result) {
                onGoingTermsOfServiceRequest = null;

                if (error !== undefined) {
                    error(result);
                }
            }
        });

        onGoingTermsOfServiceRequest = request;
    }



    _.extend(exports, {
        validateVatNumber: validateVatNumber,
        getTermsOfService: getTermsOfService
    });

})(Atomia.Api.Checkout, _, Atomia.Utils);
