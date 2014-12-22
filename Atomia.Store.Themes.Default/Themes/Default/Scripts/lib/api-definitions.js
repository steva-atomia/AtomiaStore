(function (request) {
    'use strict';

    request.define('Domains.FindDomains', 'ajax', {
        url: '/Products/FindProducts/',
        dataType: 'json',
        type: 'GET',
        decoder: 'jsend'
    });

    request.define('Cart.AddItem', 'ajax', {
        url: '/Cart/AddItem',
        dataType: 'json',
        type: 'POST',
        decoder: 'jsend'
    });


} (amplify.request));
