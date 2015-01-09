/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.Api = Atomia.Api || {};
Atomia.Api._unbound = Atomia.Api._unbound || {};
/* jshint +W079 */

Atomia.Api._unbound.Domains = function (_, amplify) {
    'use strict';

    function findDomains(searchTerm, success, error) {
        var requestData = {
            'searchQuery.Query': searchTerm
        };

        amplify.request({
            resourceId: 'Domains.FindDomains',
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

    return {
        findDomains: findDomains
    };
};

Atomia.Api.Domains = Atomia.Api._unbound.Domains(_, amplify);
