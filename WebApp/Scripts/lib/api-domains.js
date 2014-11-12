var Atomia = Atomia || {};

Atomia.Domains = (function ($, request) {
    'use strict'
    
    function findDomains(searchTerms, callback) {
        var data = {};

        $.each(searchTerms, function (i, searchTerm) {
            data["SearchTerms[" + i + "]"] = searchTerm;
        });

        request({
            resourceId: "Domains.FindDomains",
            data: data,
            success: function (data) {
                if (callback) {
                    callback(data);
                }
            },
            error: function (data, status) {
                console.log(data.message);
            }
        });
    }
    
    return {
        findDomains: findDomains
    };

} (jQuery, amplify.request));
