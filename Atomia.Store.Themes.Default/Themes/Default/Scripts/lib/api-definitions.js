(function (amplify) {
    'use strict';

    amplify.request.define('Domains.FindDomains', 'ajax', {
        url: '/Domains/FindDomains/',
        dataType: 'json',
        type: 'GET',
        decoder: 'jsend'
    });

    amplify.request.define('Cart.AddItem', 'ajax', {
        url: '/Cart/AddItem',
        dataType: 'json',
        type: 'POST',
        decoder: 'jsend'
    });

    amplify.request.define('Cart.RemoveItem', 'ajax', {
        url: '/Cart/RemoveItem',
        dataType: 'json',
        type: 'POST',
        decoder: 'jsend'
    });

    amplify.request.define('Cart.SetItemAttribute', 'ajax', {
        url: '/Cart/SetItemAttribute',
        dataType: 'json',
        type: 'POST',
        decoder: 'jsend'
    });

    amplify.request.define('Cart.RemoveItemAttribute', 'ajax', {
        url: '/Cart/RemoveItemAttribute',
        dataType: 'json',
        type: 'POST',
        decoder: 'jsend'
    });


} (amplify));
