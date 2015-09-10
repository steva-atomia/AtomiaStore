
// Setup resourceIds to use with Atomia.utils.request
; (function (amplify) {
    'use strict';

    // Monkey-patch the builtin jsend decoder to handle invalid data better.
    amplify.request.decoders.jsend = function (data, status, ampXHR, success, error) {
        if (ampXHR.statusText === 'abort') {
            return;
        }

        if (data && data.status === 'success') {
            success(data.data);
        } else if (data && data.status === 'fail') {
            error(data.data, 'fail');
        } else if (data && data.status === 'error') {
            delete data.status;
            error(data, 'error');
        } else {
            error(null, 'error');
        }
    };
    
    amplify.request.define('Domains.FindDomains', 'ajax', {
        url: '/Domains/FindDomains/',
        dataType: 'json',
        type: 'POST',
        decoder: 'jsend'
    });

    amplify.request.define('Domains.CheckStatus', 'ajax', {
        url: '/Domains/CheckStatus/',
        dataType: 'json',
        type: 'POST',
        decoder: 'jsend'
    });

    amplify.request.define('Cart.RecalculateCart', 'ajax', {
        url: '/Cart/RecalculateCart',
        dataType: 'json',
        type: 'POST',
        decoder: 'jsend'
    });

    amplify.request.define('Checkout.ValidateVatNumber', 'ajax', {
        url: '/Checkout/ValidateVatNumber',
        dataType: 'json',
        type: 'POST',
        decoder: 'jsend'
    });
} (amplify));
