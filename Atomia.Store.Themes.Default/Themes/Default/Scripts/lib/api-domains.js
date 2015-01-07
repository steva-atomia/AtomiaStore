/* jshint -W079 */
var Atomia = Atomia || {};
Atomia._unbound = Atomia._unbound || {};
/* jshint +W079 */

Atomia._unbound.Domains = function (_, request) {
    'use strict';

    function findDomains(searchTerm, success, error) {
        var requestData = {
            'searchQuery.Query': searchTerm
        };

        request({
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

Atomia.Domains = Atomia._unbound.Domains(_, amplify.request);
