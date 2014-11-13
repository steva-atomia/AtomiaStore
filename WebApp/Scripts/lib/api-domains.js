﻿var Atomia = Atomia || {};

Atomia.Domains = (function (_, request) {
    'use strict';

    function validateSearchResults(data, success) {
        var allValidAttribs = _.all(data, function (r) {
            return r.hasOwnProperty('DomainName') &&
                    r.hasOwnProperty('CurrencyCode') && 
                    r.hasOwnProperty('Price');
        });

        if (allValidAttribs) {
            success(data);
        }
        else {
            throw 'Missing field in results: expected DomainName, CurrencyCode and Price';
        }
    }

    function findDomains(searchTerm, callback) {
        var data = {
            "SearchTerm": searchTerm
        };

        request({
            resourceId: 'Domains.FindDomains',
            data: data,
            success: function (data) {
                if (callback) {
                    validateSearchResults(data, callback);
                }
            },
            error: function (data) {
                console.log(data.message);
            }
        });
    }

    return {
        findDomains: findDomains
    };

} (_, amplify.request));